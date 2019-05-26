/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FaceCat {
    /// <summary>
    /// 字符行
    /// </summary>
    public struct FCWordLine {
        /// <summary>
        /// 创建行
        /// </summary>
        /// <param name="start">开始索引</param>
        /// <param name="end">结束索引</param>
        public FCWordLine(int start, int end) {
            m_start = start;
            m_end = end;
        }

        /// <summary>
        /// 结束索引
        /// </summary>
        public int m_end;

        /// <summary>
        /// 开始索引
        /// </summary>
        public int m_start;
    }

    /// <summary>
    /// 文本框控件
    /// </summary>
    public class FCTextBox : FCDiv {
        /// <summary>
        /// 创建控件
        /// </summary>
        public FCTextBox() {
            this.Cursor = FCCursors.IBeam;
            FCSize size = new FCSize(100, 20);
            Size = size;
            TabStop = true;
        }

        /// <summary>
        /// 键盘是否按下
        /// </summary>
        protected bool m_isKeyDown;

        /// <summary>
        /// 是否触摸按下
        /// </summary>
        protected bool m_isTouchDown;

        /// <summary>
        /// 横向偏移量
        /// </summary>
        protected int m_offsetX = 0;

        /// <summary>
        /// 文字矩形范围
        /// </summary>
        protected ArrayList<FCRectF> m_ranges = new ArrayList<FCRectF>();

        /// <summary>
        /// 重做栈
        /// </summary>
        protected Stack<String> m_redoStack = new Stack<String>();

        /// <summary>
        /// 是否显示光标
        /// </summary>
        protected bool m_showCursor = false;

        /// <summary>
        /// 开始移动的坐标
        /// </summary>
        protected int m_startMovingIndex = -1;

        /// <summary>
        /// 结束移动的坐标
        /// </summary>
        protected int m_stopMovingIndex = -1;

        /// <summary>
        /// 文字是否改变
        /// </summary>
        protected bool m_textChanged = false;

        /// <summary>
        /// 光标闪烁频率
        /// </summary>
        protected int TICK = 500;

        /// <summary>
        /// 秒表ID
        /// </summary>
        private int m_timerID = getNewTimerID();

        /// <summary>
        /// 撤销栈
        /// </summary>
        protected Stack<String> m_undoStack = new Stack<String>();

        /// <summary>
        /// 文字大小
        /// </summary>
        protected ArrayList<FCSizeF> m_wordsSize = new ArrayList<FCSizeF>();

        /// <summary>
        /// 获取行数
        /// </summary>
        public virtual int LinesCount {
            get { return m_lines.size(); }
        }

        protected int m_lineHeight = 20;

        /// <summary>
        /// 获取或设置行高
        /// </summary>
        public virtual int LineHeight {
            get { return m_lineHeight; }
            set { m_lineHeight = value; }
        }

        protected ArrayList<FCWordLine> m_lines = new ArrayList<FCWordLine>();

        /// <summary>
        /// 获取行数
        /// </summary>
        public virtual ArrayList<FCWordLine> Lines {
            get { return m_lines; }
        }

        protected bool m_multiline = false;

        /// <summary>
        /// 获取或设置是否多行显示
        /// </summary>
        public virtual bool Multiline {
            get { return m_multiline; }
            set {
                if (m_multiline != value) {
                    m_multiline = value;
                    m_textChanged = true;
                }
                ShowVScrollBar = m_multiline;
            }
        }

        protected char m_passwordChar;

        /// <summary>
        /// 获取或设置密码字符
        /// </summary>
        public virtual char PasswordChar {
            get { return m_passwordChar; }
            set {
                m_passwordChar = value;
                m_textChanged = true;
            }
        }

        protected bool m_readOnly = false;

        /// <summary>
        /// 获取或设置是否只读
        /// </summary>
        public virtual bool ReadOnly {
            get { return m_readOnly; }
            set { m_readOnly = value; }
        }

        protected bool m_rightToLeft;

        /// <summary>
        /// 获取或设置是否从右向左绘制
        /// </summary>
        public virtual bool RightToLeft {
            get { return m_rightToLeft; }
            set {
                m_rightToLeft = value;
                m_textChanged = true;
            }
        }

        /// <summary>
        /// 获取选中的文字
        /// </summary>
        public virtual String SelectionText {
            get {
                String text = Text;
                if (text == null) {
                    text = "";
                }
                int textLength = text.Length;
                if (textLength > 0 && m_selectionStart != textLength) {
                    String selectedText = text.Substring(m_selectionStart, m_selectionLength);
                    return selectedText;
                }
                return String.Empty;
            }
        }

        protected long m_selectionBackColor = FCColor.argb(10, 36, 106);

        /// <summary>
        /// 获取或设置选中的背景色
        /// </summary>
        public virtual long SelectionBackColor {
            get { return m_selectionBackColor; }
            set { m_selectionBackColor = value; }
        }

        protected long m_selectionTextColor = FCColor.argb(255, 255, 255);

        /// <summary>
        /// 获取或设置选中的前景色
        /// </summary>
        public virtual long SelectionTextColor {
            get { return m_selectionTextColor; }
            set { m_selectionTextColor = value; }
        }

        protected int m_selectionLength;

        /// <summary>
        /// 获取或设置选中长度
        /// </summary>
        public virtual int SelectionLength {
            get { return m_selectionLength; }
            set { m_selectionLength = value; }
        }

        protected int m_selectionStart = -1;

        /// <summary>
        /// 获取或设置选中开始位置
        /// </summary>
        public virtual int SelectionStart {
            get { return m_selectionStart; }
            set {
                m_selectionStart = value;
                if (m_selectionStart > Text.Length) {
                    m_selectionStart = Text.Length;
                }
            }
        }

        protected String m_tempText;

        /// <summary>
        /// 获取或设置临时文字
        /// </summary>
        public virtual String TempText {
            get { return m_tempText; }
            set { m_tempText = value; }
        }

        protected long m_tempTextColor = FCColor.DisabledText;

        /// <summary>
        /// 获取或设置临时文字的颜色
        /// </summary>
        public virtual long TempTextColor {
            get { return m_tempTextColor; }
            set { m_tempTextColor = value; }
        }

        protected FCHorizontalAlign m_textAlign = FCHorizontalAlign.Left;

        /// <summary>
        /// 获取或设置内容的横向排列样式
        /// </summary>
        public virtual FCHorizontalAlign TextAlign {
            get { return m_textAlign; }
            set { m_textAlign = value; }
        }

        protected bool m_wordWrap = false;

        /// <summary>
        /// 获取或设置多行编辑控件是否启动换行
        /// </summary>
        public virtual bool WordWrap {
            get { return m_wordWrap; }
            set {
                if (m_wordWrap != value) {
                    m_wordWrap = value;
                    m_textChanged = true;
                }
                ShowHScrollBar = !m_wordWrap;
            }
        }

        /// <summary>
        /// 判断是否可以重复
        /// </summary>
        /// <returns>是否可以重复</returns>
        public bool canRedo() {
            if (m_redoStack.Count > 0) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断是否可以撤销
        /// </summary>
        /// <returns>是否可以撤销</returns>
        public bool canUndo() {
            if (m_undoStack.Count > 0) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void clearRedoUndo() {
            m_undoStack.Clear();
            m_redoStack.Clear();
        }

        /// <summary>
        /// 光标向下移动
        /// </summary>
        public void cursorDown() {
            FCHost host = Native.Host;
            int scol = -1, srow = -1;
            int rangeSize = m_ranges.size();
            int start = m_selectionStart + m_selectionLength < rangeSize - 1 ? m_selectionStart + m_selectionLength : rangeSize - 1;
            if (host.isKeyPress(0x10)) {
                start = m_stopMovingIndex;
            }
            else {
                if (m_selectionLength > 0) {
                    m_selectionLength = 1;
                }
            }
            int lineCount = m_lines.size();
            bool check = false;
            for (int i = 0; i < lineCount; i++) {
                FCWordLine line = m_lines[i];
                for (int j = line.m_start; j <= line.m_end; j++) {
                    if (j >= start && j < rangeSize) {
                        int col = j - line.m_start;
                        if (j == start) {
                            if (i != 0 && j == line.m_start) {
                                check = true;
                                srow = i - 1;
                                scol = line.m_end + 1;
                            }
                            else {
                                if (i != lineCount - 1) {
                                    check = true;
                                    int idx = j - line.m_start;
                                    scol = m_lines[i + 1].m_start + idx + 1;
                                    srow = i;
                                    continue;
                                }
                            }
                        }
                        if (check) {
                            if (i == srow + 1) {
                                if (host.isKeyPress(0x10)) {
                                    setMovingIndex(m_startMovingIndex, j);
                                }
                                else {
                                    if (scol > line.m_end) {
                                        scol = line.m_end + 1;
                                    }
                                    m_selectionStart = scol;
                                    m_selectionLength = 0;
                                    m_startMovingIndex = m_selectionStart;
                                    m_stopMovingIndex = m_selectionStart;
                                }
                                m_showCursor = true;
                                startTimer(m_timerID, TICK);
                                return;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 光标移动到最右端
        /// </summary>
        public void cursorEnd() {
            FCHost host = Native.Host;
            int rangeSize = m_ranges.size();
            int start = m_selectionStart + m_selectionLength < rangeSize - 1 ? m_selectionStart + m_selectionLength : rangeSize - 1;
            if (host.isKeyPress(0x10)) {
                start = m_stopMovingIndex;
            }
            int lineCount = m_lines.size();
            for (int i = 0; i < lineCount; i++) {
                FCWordLine line = m_lines[i];
                for (int j = line.m_start; j <= line.m_end; j++) {
                    if (j == start) {
                        if (j == line.m_start && i > 0) {
                            line = m_lines[i - 1];
                        }
                        if (host.isKeyPress(0x10)) {
                            setMovingIndex(m_startMovingIndex, line.m_end + 1);
                        }
                        else {
                            m_selectionStart = line.m_end + 1;
                            m_selectionLength = 0;
                            m_startMovingIndex = m_selectionStart;
                            m_stopMovingIndex = m_selectionStart;
                        }
                        m_showCursor = true;
                        startTimer(m_timerID, TICK);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 光标移动到最左端
        /// </summary>
        public void cursorHome() {
            FCHost host = Native.Host;
            int rangeSize = m_ranges.size();
            int start = m_selectionStart < rangeSize - 1 ? m_selectionStart : rangeSize - 1;
            if (host.isKeyPress(0x10)) {
                start = m_stopMovingIndex;
            }
            int lineCount = m_lines.size();
            for (int i = 0; i < lineCount; i++) {
                FCWordLine line = m_lines[i];
                for (int j = line.m_start; j <= line.m_end; j++) {
                    if (j == start) {
                        if (j == line.m_start && i > 0) {
                            line = m_lines[i - 1];
                        }
                        if (host.isKeyPress(0x10)) {
                            setMovingIndex(m_startMovingIndex, line.m_start + 1);
                        }
                        else {
                            m_selectionStart = line.m_start + 1;
                            m_selectionLength = 0;
                            m_startMovingIndex = m_selectionStart;
                            m_stopMovingIndex = m_selectionStart;
                        }
                        m_showCursor = true;
                        startTimer(m_timerID, TICK);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 光标向左移动
        /// </summary>
        public void cursorLeft() {
            FCHost host = Native.Host;
            if (host.isKeyPress(0x10)) {
                setMovingIndex(m_startMovingIndex, m_stopMovingIndex - 1);
            }
            else {
                if (m_selectionStart > 0) {
                    m_selectionStart -= 1;
                }
                m_selectionLength = 0;
                m_startMovingIndex = m_selectionStart;
                m_stopMovingIndex = m_selectionStart;
            }
        }

        /// <summary>
        /// 光标向右移动
        /// </summary>
        public void cursorRight() {
            FCHost host = Native.Host;
            if (host.isKeyPress(0x10)) {
                setMovingIndex(m_startMovingIndex, m_stopMovingIndex + 1);
            }
            else {
                int rangeSize = m_ranges.size();
                int start = m_selectionStart + m_selectionLength < rangeSize - 1 ? m_selectionStart + m_selectionLength : rangeSize - 1;
                if (start < rangeSize) {
                    m_selectionStart = start + 1;
                }
                m_selectionLength = 0;
                m_startMovingIndex = m_selectionStart;
                m_stopMovingIndex = m_selectionStart;
            }
        }

        /// <summary>
        /// 光标向上移动
        /// </summary>
        public void cursorUp() {
            FCHost host = Native.Host;
            int scol = -1, srow = -1;
            int rangeSize = m_ranges.size();
            int start = m_selectionStart < rangeSize - 1 ? m_selectionStart : rangeSize - 1;
            if (host.isKeyPress(0x10)) {
                start = m_stopMovingIndex;
            }
            else {
                if (m_selectionLength > 0) {
                    m_selectionLength = 1;
                }
            }
            int lineCount = m_lines.size();
            bool check = false;
            for (int i = lineCount - 1; i >= 0; i--) {
                FCWordLine line = m_lines[i];
                for (int j = line.m_end; j >= line.m_start; j--) {
                    if (j >= 0 && j <= start) {
                        int col = j - line.m_start;
                        if (i != 0 && j == start) {
                            check = true;
                            if (i != lineCount - 1 && j == line.m_start) {
                                srow = i;
                                scol = m_lines[i - 1].m_start;
                            }
                            else {
                                int idx = j - line.m_start;
                                scol = m_lines[i - 1].m_start + idx - 1;
                                if (scol < 0) {
                                    scol = 0;
                                }
                                srow = i;
                            }
                            continue;
                        }
                        if (check) {
                            if (i == srow - 1 && col <= scol) {
                                if (host.isKeyPress(0x10)) {
                                    setMovingIndex(m_startMovingIndex, j);
                                }
                                else {
                                    if (scol > line.m_end) {
                                        scol = line.m_end + 1;
                                    }
                                    m_selectionStart = scol;
                                    m_selectionLength = 0;
                                    m_startMovingIndex = m_selectionStart;
                                    m_stopMovingIndex = m_selectionStart;
                                }
                                m_showCursor = true;
                                startTimer(m_timerID, TICK);
                                return;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 删除字符
        /// </summary>
        public virtual void deleteWord() {
            String text = Text;
            if (text == null) {
                text = "";
            }
            int textLength = text.Length;
            if (textLength > 0) {
                int oldLines = m_lines.size();
                String left = "", right = "";
                int rightIndex = -1;
                if (m_selectionLength > 0) {
                    left = m_selectionStart > 0 ? text.Substring(0, m_selectionStart) : "";
                    rightIndex = m_selectionStart + m_selectionLength;
                }
                else {
                    left = m_selectionStart > 0 ? text.Substring(0, m_selectionStart - 1) : "";
                    rightIndex = m_selectionStart + m_selectionLength;
                    if (m_selectionStart > 0) {
                        m_selectionStart -= 1;
                    }
                }
                if (rightIndex < textLength) {
                    right = text.Substring(rightIndex);
                }
                String newText = left + right;
                m_text = newText;
                textLength = newText.Length;
                if (textLength == 0) {
                    m_selectionStart = 0;
                }
                else {
                    if (m_selectionStart > textLength) {
                        m_selectionStart = textLength;
                    }
                }
                m_selectionLength = 0;
            }
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public override void delete() {
            if (!IsDeleted) {
                stopTimer(m_timerID);
                m_lines.clear();
                m_ranges.clear();
                m_redoStack.Clear();
                m_undoStack.Clear();
                m_wordsSize.clear();
            }
            base.delete();
        }

        /// <summary>
        /// 获取内容的高度
        /// </summary>
        /// <returns>高度</returns>
        public override int getContentHeight() {
            int hmax = base.getContentHeight();
            int cheight = 0;
            int rangeSize = m_ranges.size();
            for (int i = 0; i < rangeSize; i++) {
                if (cheight < m_ranges[i].bottom) {
                    cheight = (int)m_ranges[i].bottom;
                }
            }
            return hmax > cheight ? hmax : cheight;
        }

        /// <summary>
        /// 获取内容的宽度
        /// </summary>
        /// <returns>宽度</returns>
        public override int getContentWidth() {
            int wmax = base.getContentWidth();
            int cwidth = 0;
            int rangeSize = m_ranges.size();
            for (int i = 0; i < rangeSize; i++) {
                if (cwidth < m_ranges[i].right) {
                    cwidth = (int)m_ranges[i].right;
                }
            }
            return wmax > cwidth ? wmax : cwidth;
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "TextBox";
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "lineheight") {
                type = "int";
                value = FCStr.convertIntToStr(LineHeight);
            }
            else if (name == "multiline") {
                type = "bool";
                value = FCStr.convertBoolToStr(Multiline);
            }
            else if (name == "passwordchar") {
                type = "text";
                value = PasswordChar.ToString();
            }
            else if (name == "readonly") {
                type = "bool";
                value = FCStr.convertBoolToStr(ReadOnly);
            }
            else if (name == "righttoleft") {
                type = "bool";
                value = FCStr.convertBoolToStr(RightToLeft);
            }
            else if (name == "selectionbackcolor") {
                type = "color";
                value = FCStr.convertColorToStr(SelectionBackColor);
            }
            else if (name == "selectiontextcolor") {
                type = "color";
                value = FCStr.convertColorToStr(SelectionTextColor);
            }
            else if (name == "temptext") {
                type = "text";
                value = TempText;
            }
            else if (name == "temptextcolor") {
                type = "color";
                value = FCStr.convertColorToStr(TempTextColor);
            }
            else if (name == "textalign") {
                type = "enum:FCHorizontalAlign";
                value = FCStr.convertHorizontalAlignToStr(TextAlign);
            }
            else if (name == "wordwrap") {
                type = "bool";
                value = FCStr.convertBoolToStr(WordWrap);
            }
            else {
                base.getProperty(name, ref value, ref type);
            }
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns>属性名称列表</returns>
        public override ArrayList<String> getPropertyNames() {
            ArrayList<String> propertyNames = base.getPropertyNames();
            propertyNames.AddRange(new String[] { "LineHeight", "Multiline", "PasswordChar", "ReadOnly", "RightToLeft", "SelectionBackColor", "SelectionTextColor", "TempText", "TempTextColor", "TextAlign", "WordWrap" });
            return propertyNames;
        }

        /// <summary>
        /// 判断字符索引所在行是否可见
        /// </summary>
        /// <param name="index">字符索引</param>
        /// <param name="visiblePercent">可见百分比</param>
        /// <returns>是否可见</returns>
        public bool isLineVisible(int index, double visiblePercent) {
            int rangeSize = m_ranges.size();
            if (rangeSize > 0) {
                if (index >= 0 && index < rangeSize) {
                    int top = 0, scrollV = 0, sch = 0;
                    FCHScrollBar hScrollBar = HScrollBar;
                    FCVScrollBar vScrollBar = VScrollBar;
                    if (hScrollBar != null && hScrollBar.Visible) {
                        sch = hScrollBar.Height;
                    }
                    if (vScrollBar != null && vScrollBar.Visible) {
                        scrollV = -vScrollBar.Pos;
                    }
                    top = scrollV;
                    int cell = 1;
                    int floor = Height - cell - sch - 1;
                    FCRectF indexRect = m_ranges[index];
                    int indexTop = (int)indexRect.top + scrollV;
                    int indexBottom = (int)indexRect.bottom + scrollV;
                    return lineVisible(indexTop, indexBottom, cell, floor, m_lineHeight, visiblePercent);
                }
            }
            return false;
        }


        /// <summary>
        /// 插入字符
        /// </summary>
        /// <param name="str">字符串</param>
        public virtual void insertWord(String str) {
            String text = Text;
            if (text == null) {
                text = "";
            }
            if (text.Length == 0 || m_selectionStart == text.Length) {
                text = text + str;
                m_text = text;
                m_selectionStart = text.Length;
            }
            else {
                int sIndex = m_selectionStart > text.Length ? text.Length : m_selectionStart;
                String left = sIndex > 0 ? text.Substring(0, sIndex) : "";
                String right = "";
                int rightIndex = m_selectionStart + (m_selectionLength == 0 ? 0 : m_selectionLength);
                if (rightIndex < text.Length) {
                    right = text.Substring(rightIndex);
                }
                text = left + str + right;
                m_text = text;
                m_selectionStart += str.Length;
                if (m_selectionStart > text.Length) {
                    m_selectionStart = text.Length;
                }
                m_selectionLength = 0;
            }
        }

        /// <summary>
        /// 判断行是否可见
        /// </summary>
        /// <param name="indexTop"></param>
        /// <param name="indexBottom"></param>
        /// <param name="cell"></param>
        /// <param name="floor"></param>
        /// <param name="lineHeight"></param>
        /// <param name="visiblePercent"></param>
        /// <returns></returns>
        public virtual bool lineVisible(int indexTop, int indexBottom, int cell, int floor, int lineHeight, double visiblePercent) {
            if (indexTop < cell) {
                indexTop = cell;
            }
            else if (indexTop > floor) {
                indexTop = floor;
            }
            if (indexBottom < cell) {
                indexBottom = cell;
            }
            else if (indexBottom > floor) {
                indexBottom = floor;
            }
            if (indexBottom - indexTop > lineHeight * visiblePercent) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 文本输入方法
        /// </summary>
        /// <param name="ch">文字</param>
        public override void onChar(char ch) {
            if (!m_readOnly) {
                base.onChar(ch);
                FCHost host = Native.Host;
                if (!host.isKeyPress(0x11)) {
                    int oldLines = m_lines.size();
                    if (ch != 8 || (!m_multiline && ch == 13)) {
                        String redotext = Text;
                        insertWord(ch.ToString());
                        onTextChanged();
                        if (m_textChanged) {
                            if (redotext != null) {
                                m_undoStack.Push(redotext);
                            }
                        }
                    }
                    invalidate();
                    int newLines = m_lines.size();
                    if (newLines != oldLines) {
                        FCVScrollBar vScrollBar = VScrollBar;
                        if (vScrollBar != null) {
                            vScrollBar.Pos += m_lineHeight * (newLines - oldLines);
                            invalidate();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 复制文字
        /// </summary>
        public override void onCopy() {
            String selectionText = SelectionText;
            if (selectionText != null && selectionText.Length > 0) {
                FCHost host = Native.Host;
                host.copy(selectionText);
                base.onCopy();
            }
        }

        /// <summary>
        /// 剪切
        /// </summary>
        public override void onCut() {
            if (!m_readOnly) {
                onCopy();
                int oldLines = m_lines.size();
                String redotext = Text;
                deleteWord();
                onTextChanged();
                if (m_textChanged) {
                    if (redotext != null) {
                        m_undoStack.Push(redotext);
                    }
                }
                invalidate();
                int newLines = m_lines.size();
                if (newLines != oldLines) {
                    FCVScrollBar vScrollBar = VScrollBar;
                    if (vScrollBar != null) {
                        vScrollBar.Pos += m_lineHeight * (newLines - oldLines);
                        invalidate();
                    }
                }
                base.onCut();
            }
        }

        /// <summary>
        /// 获取焦点方法
        /// </summary>
        public override void onGotFocus() {
            base.onGotFocus();
            m_showCursor = true;
            invalidate();
            startTimer(m_timerID, TICK);
        }

        /// <summary>
        /// 键盘方法
        /// </summary>
        /// <param name="key">按键</param>
        public override void onKeyDown(char key) {
            m_isKeyDown = true;
            FCHost host = Native.Host;
            if (host.isKeyPress(0x11)) {
                base.onKeyDown(key);
                //全选
                if (key == 65) {
                    selectAll();
                }
                //重做
                else if (key == 89) {
                    redo();
                }
                //撤销
                else if (key == 90) {
                    undo();
                }
            }
            else {
                if (key >= 35 && key <= 40) {
                    if (key == 38 || key == 40) {
                        callKeyEvents(FCEventID.KEYDOWN, key);
                        if (m_lines.size() > 1) {
                            int offset = 0;
                            //光标向上移动
                            if (key == 38) {
                                cursorUp();
                                if (!isLineVisible(m_stopMovingIndex, 0.6)) {
                                    offset = -m_lineHeight;
                                }
                            }
                            //光标向下移动
                            else if (key == 40) {
                                cursorDown();
                                if (!isLineVisible(m_stopMovingIndex, 0.6)) {
                                    offset = m_lineHeight;
                                }
                            }
                            FCVScrollBar vScrollBar = VScrollBar;
                            if (vScrollBar != null && vScrollBar.Visible) {
                                vScrollBar.Pos += offset;
                                vScrollBar.update();
                            }
                        }
                    }
                    else {
                        base.onKeyDown(key);
                        //光标移动到最右端
                        if (key == 35) {
                            cursorEnd();
                        }
                        //光标移动到最左端
                        else if (key == 36) {
                            cursorHome();
                        }
                        //光标向左移动
                        else if (key == 37) {
                            cursorLeft();
                        }
                        //光标向右移动
                        else if (key == 39) {
                            cursorRight();
                        }
                    }
                }
                else {
                    base.onKeyDown(key);
                    //取消焦点
                    if (key == 27) {
                        Focused = false;
                    }
                    //删除
                    else if (key == 8 || key == 46) {
                        if (!m_readOnly) {
                            int oldLines = m_lines.size();
                            String redotext = Text;
                            deleteWord();
                            onTextChanged();
                            if (m_textChanged) {
                                if (redotext != null) {
                                    m_undoStack.Push(redotext);
                                }
                            }
                            invalidate();
                            int newLines = m_lines.size();
                            if (newLines != oldLines) {
                                FCVScrollBar vScrollBar = VScrollBar;
                                if (vScrollBar != null) {
                                    vScrollBar.Pos += m_lineHeight * (newLines - oldLines);
                                    invalidate();
                                }
                            }
                        }
                    }
                }
            }
            invalidate();
        }

        /// <summary>
        /// 键盘抬起方法
        /// </summary>
        /// <param name="key">按键</param>
        public override void onKeyUp(char key) {
            base.onKeyUp(key);
            FCHost host = Native.Host;
            if (!host.isKeyPress(0x10) && !m_isTouchDown) {
                m_startMovingIndex = m_selectionStart;
                m_stopMovingIndex = m_selectionStart;
            }
            m_isKeyDown = false;
        }

        /// <summary>
        /// 丢失焦点方法
        /// </summary>
        public override void onLostFocus() {
            base.onLostFocus();
            stopTimer(m_timerID);
            m_isKeyDown = false;
            m_showCursor = false;
            m_selectionLength = 0;
            invalidate();
        }

        /// <summary>
        /// 触摸按下方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchDown(FCTouchInfo touchInfo) {
            base.onTouchDown(touchInfo);
            if (touchInfo.m_firstTouch) {
                FCPoint mp = touchInfo.m_firstPoint;
                //单击
                if (touchInfo.m_clicks == 1) {
                    int sIndex = -1;
                    int linesCount = m_lines.size();
                    m_selectionLength = 0;
                    m_startMovingIndex = -1;
                    m_stopMovingIndex = -1;
                    if (linesCount > 0) {
                        FCHScrollBar hScrollBar = HScrollBar;
                        FCVScrollBar vScrollBar = VScrollBar;
                        int scrollH = (hScrollBar != null && hScrollBar.Visible) ? hScrollBar.Pos : 0;
                        int scrollV = (vScrollBar != null && vScrollBar.Visible) ? vScrollBar.Pos : 0;
                        scrollH += m_offsetX;
                        int x = mp.x + scrollH, y = mp.y + scrollV;
                        FCRectF lastRange = new FCRectF();
                        int rangeSize = m_ranges.size();
                        if (rangeSize > 0) {
                            lastRange = m_ranges[rangeSize - 1];
                        }
                        for (int i = 0; i < linesCount; i++) {
                            FCWordLine line = m_lines[i];
                            for (int j = line.m_start; j <= line.m_end; j++) {
                                FCRectF rect = m_ranges[j];
                                if (i == linesCount - 1) {
                                    rect.bottom += 3;
                                }
                                if (y >= rect.top && y <= rect.bottom) {
                                    float sub = (rect.right - rect.left) / 2;
                                    if ((x >= rect.left - sub && x <= rect.right - sub)
                                        || (j == line.m_start && x <= rect.left + sub)
                                        || (j == line.m_end && x >= rect.right - sub)) {
                                        if (j == line.m_end && x >= rect.right - sub) {
                                            sIndex = j + 1;
                                        }
                                        else {
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
                    }
                    else {
                        m_selectionStart = 0;
                    }
                    m_startMovingIndex = m_selectionStart;
                    m_stopMovingIndex = m_selectionStart;
                }
                //双击
                else if (touchInfo.m_clicks == 2) {
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

        /// <summary>
        /// 触摸移动方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchMove(FCTouchInfo touchInfo) {
            base.onTouchMove(touchInfo);
            if (m_isTouchDown) {
                int linesCount = m_lines.size();
                if (linesCount > 0) {
                    int eIndex = -1;
                    FCHScrollBar hScrollBar = HScrollBar;
                    FCVScrollBar vScrollBar = VScrollBar;
                    int scrollH = (hScrollBar != null && hScrollBar.Visible) ? hScrollBar.Pos : 0;
                    int scrollV = (vScrollBar != null && vScrollBar.Visible) ? vScrollBar.Pos : 0;
                    scrollH += m_offsetX;
                    FCPoint point = touchInfo.m_firstPoint;
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
                        lastRange = m_ranges[rangeSize - 1];
                    }
                    for (int i = 0; i < linesCount; i++) {
                        FCWordLine line = m_lines[i];
                        for (int j = line.m_start; j <= line.m_end; j++) {
                            FCRectF rect = m_ranges[j];
                            if (i == linesCount - 1) {
                                rect.bottom += 3;
                            }
                            if (eIndex == -1) {
                                if (y >= rect.top && y <= rect.bottom) {
                                    float sub = (rect.right - rect.left) / 2;
                                    if ((x >= rect.left - sub && x <= rect.right - sub)
                                        || (j == line.m_start && x <= rect.left + sub)
                                        || (j == line.m_end && x >= rect.right - sub)) {
                                        if (j == line.m_end && x >= rect.right - sub) {
                                            eIndex = j + 1;
                                        }
                                        else {
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
                    }
                    else {
                        m_selectionStart = m_startMovingIndex < m_stopMovingIndex ? m_startMovingIndex : m_stopMovingIndex;
                        m_selectionLength = Math.Abs(m_startMovingIndex - m_stopMovingIndex);
                    }
                    invalidate();
                }
            }
        }

        /// <summary>
        /// 触摸抬起方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onTouchUp(FCTouchInfo touchInfo) {
            m_isTouchDown = false;
            base.onTouchUp(touchInfo);
        }

        /// <summary>
        /// 重绘前景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintForeground(FCPaint paint, FCRect clipRect) {
            int width = Width, height = Height;
            if (width > 0 && height > 0) {
                int lineHeight = m_multiline ? m_lineHeight : height;
                FCPadding padding = Padding;
                FCHost host = Native.Host;
                FCRect rect = new FCRect(0, 0, width, height);
                FCFont font = Font;
                FCSizeF tSize = paint.textSizeF(" ", font);
                FCHScrollBar hScrollBar = HScrollBar;
                FCVScrollBar vScrollBar = VScrollBar;
                int vWidth = (vScrollBar != null && vScrollBar.Visible) ? (vScrollBar.Width - padding.left - padding.right) : 0;
                int scrollH = ((hScrollBar != null && hScrollBar.Visible) ? hScrollBar.Pos : 0);
                int scrollV = ((vScrollBar != null && vScrollBar.Visible) ? vScrollBar.Pos : 0);
                float strX = padding.left + 1;
                //绘制文字
                String text = Text;
                if (text == null) {
                    text = "";
                }
                int length = text.Length;
                FCSizeF bSize = paint.textSizeF("0", font);
                if (m_textChanged) {
                    int line = 0, count = 0;
                    //获取绘制区域
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
                        char ch = text[i];
                        String dch = ch.ToString();
                        //制表符
                        if (ch == 9) {
                            int addCount = 4 - count % 4;
                            tSize.cx = bSize.cx * addCount;
                            tSize.cy = bSize.cy;
                            count += addCount;
                        }
                        else {
                            //密码替换
                            if (m_passwordChar != 0) {
                                dch = m_passwordChar.ToString();
                            }
                            tSize = paint.textSizeF(dch, font);
                            if (ch == 10) {
                                tSize.cx = 0;
                            }
                            count++;
                        }
                        //判断是否多行
                        if (m_multiline) {
                            bool isNextLine = false;
                            if (ch == 13) {
                                isNextLine = true;
                                tSize.cx = 0;
                            }
                            else {
                                //是否自动换行
                                if (m_wordWrap) {
                                    if (strX + tSize.cx > width - vWidth) {
                                        isNextLine = true;
                                    }
                                }
                            }
                            //换行
                            if (isNextLine) {
                                count = 0;
                                line++;
                                strX = padding.left + 1;
                                m_lines.add(new FCWordLine(i, i));
                            }
                            else {
                                m_lines[line - 1] = new FCWordLine(m_lines[line - 1].m_start, i);
                            }
                        }
                        else {
                            m_lines[line - 1] = new FCWordLine(m_lines[line - 1].m_start, i);
                        }
                        if (ch > 1000) {
                            tSize.cx += 1;
                        }
                        //保存区域
                        FCRectF rangeRect = new FCRectF(strX, padding.top + (line - 1) * lineHeight, strX + tSize.cx, padding.top + line * lineHeight);
                        m_ranges.add(rangeRect);
                        m_wordsSize.add(tSize);
                        strX = rangeRect.right;
                    }
                    //从右向左
                    if (m_rightToLeft) {
                        int lcount = m_lines.size();
                        for (int i = 0; i < lcount; i++) {
                            FCWordLine ln = m_lines[i];
                            float lw = width - vWidth - (m_ranges[ln.m_end].right - m_ranges[ln.m_start].left) - 2;
                            if (lw > 0) {
                                for (int j = ln.m_start; j <= ln.m_end; j++) {
                                    FCRectF rangeRect = m_ranges[j];
                                    rangeRect.left += lw;
                                    rangeRect.right += lw;
                                    m_ranges[j] = rangeRect;
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
                //判断当前索引是否可见
                if (!m_multiline) {
                    FCRectF firstRange = new FCRectF();
                    FCRectF lastRange = new FCRectF();
                    if (rangesSize > 0) {
                        firstRange = m_ranges[0];
                        lastRange = m_ranges[rangesSize - 1];
                    }
                    scrollH -= offsetX;
                    //居中
                    if (m_textAlign == FCHorizontalAlign.Center) {
                        offsetX = -(int)(width - padding.right - (lastRange.right - firstRange.left)) / 2;
                    }
                    //远离
                    else if (m_textAlign == FCHorizontalAlign.Right) {
                        offsetX = -(int)(width - padding.right - (lastRange.right - firstRange.left) - 3);
                    }
                    //居左
                    else {
                        //显示超出边界
                        if (lastRange.right > width - padding.right) {
                            //获取总是可见的索引
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
                                FCRectF alwaysVisibleRange = m_ranges[alwaysVisibleIndex];
                                int cw = width - padding.left - padding.right;
                                if (alwaysVisibleRange.left - offsetX > cw - 10) {
                                    offsetX = (int)alwaysVisibleRange.right - cw + 3;
                                    if (offsetX < 0) {
                                        offsetX = 0;
                                    }
                                }
                                else if (alwaysVisibleRange.left - offsetX < 10) {
                                    offsetX -= (int)bSize.cx * 4;
                                    if (offsetX < 0) {
                                        offsetX = 0;
                                    }
                                }
                                if (offsetX > lastRange.right - cw) {
                                    offsetX = (int)lastRange.right - cw + 3;
                                }
                            }
                        }
                        //显示未超出边界
                        else {
                            if (m_textAlign == FCHorizontalAlign.Right) {
                                offsetX = -(int)(width - padding.right - (lastRange.right - firstRange.left) - 3);
                            }
                            else {
                                offsetX = 0;
                            }
                        }
                    }
                    m_offsetX = offsetX;
                    scrollH += m_offsetX;
                }
                int lineCount = m_lines.size();
                //绘制矩形和字符
                ArrayList<FCRectF> selectedRanges = new ArrayList<FCRectF>();
                ArrayList<FCRect> selectedWordsRanges = new ArrayList<FCRect>();
                ArrayList<char> selectedWords = new ArrayList<char>();
                for (int i = 0; i < lineCount; i++) {
                    FCWordLine line = m_lines[i];
                    for (int j = line.m_start; j <= line.m_end; j++) {
                        char ch = text[j];
                        if (ch != 9) {
                            //密码替换
                            if (m_passwordChar > 0) {
                                ch = m_passwordChar;
                            }
                        }
                        //获取绘制区域
                        FCRectF rangeRect = m_ranges[j];
                        rangeRect.left -= scrollH;
                        rangeRect.top -= scrollV;
                        rangeRect.right -= scrollH;
                        rangeRect.bottom -= scrollV;
                        FCRect rRect = new FCRect(rangeRect.left, rangeRect.top + (lineHeight - m_wordsSize[j].cy) / 2,
                            rangeRect.right, rangeRect.top + (lineHeight + m_wordsSize[j].cy) / 2);
                        if (rRect.right == rRect.left) {
                            rRect.right = rRect.left + 1;
                        }
                        //绘制文字
                        if (host.getIntersectRect(ref tempRect, ref rRect, ref rect) > 0) {
                            if (m_selectionLength > 0) {
                                if (j >= m_selectionStart && j < m_selectionStart + m_selectionLength) {
                                    selectedWordsRanges.add(rRect);
                                    selectedRanges.add(rangeRect);
                                    selectedWords.add(ch);
                                    continue;
                                }
                            }
                            paint.drawText(ch.ToString(), getPaintingTextColor(), font, rRect);
                        }
                    }
                }
                //绘制选中区域
                int selectedRangesSize = selectedRanges.size();
                if (selectedRangesSize > 0) {
                    int sIndex = 0;
                    float right = 0;
                    for (int i = 0; i < selectedRangesSize; i++) {
                        FCRectF rRect = selectedRanges[i];
                        FCRectF sRect = selectedRanges[sIndex];
                        bool newLine = rRect.top != sRect.top;
                        if (newLine || i == selectedRangesSize - 1) {
                            int eIndex = (i == selectedRangesSize - 1) ? i : i - 1;
                            FCRectF eRect = selectedRanges[eIndex];
                            FCRect unionRect = new FCRect(sRect.left, sRect.top, eRect.right + 1, sRect.bottom + 1);
                            if (newLine) {
                                unionRect.right = (int)right;
                            }
                            paint.fillRect(m_selectionBackColor, unionRect);
                            for (int j = sIndex; j <= eIndex; j++) {
                                paint.drawText(selectedWords[j].ToString(), m_selectionTextColor, font, selectedWordsRanges[j]);
                            }
                            sIndex = i;
                        }
                        right = rRect.right;
                    }
                    selectedRanges.clear();
                    selectedWords.clear();
                    selectedWordsRanges.clear();
                }
                //绘制光标
                if (Focused && !m_readOnly && m_selectionLength == 0 && (m_isKeyDown || m_showCursor)) {
                    int index = m_selectionStart;
                    if (index < 0) {
                        index = 0;
                    }
                    if (index > length) {
                        index = length;
                    }
                    //获取光标的位置
                    int cursorX = offsetX;
                    int cursorY = 0;
                    if (length > 0) {
                        if (index == 0) {
                            if (rangesSize > 0) {
                                cursorX = (int)m_ranges[0].left;
                                cursorY = (int)m_ranges[0].top;
                            }
                        }
                        else {
                            cursorX = (int)Math.Ceiling(m_ranges[index - 1].right) + 2;
                            cursorY = (int)Math.Ceiling(m_ranges[index - 1].top);
                        }
                        cursorY += lineHeight / 2 - (int)tSize.cy / 2;
                    }
                    else {
                        cursorY = lineHeight / 2 - (int)tSize.cy / 2;
                    }
                    //绘制闪烁光标
                    if (m_isKeyDown || m_showCursor) {
                        FCRect cRect = new FCRect(cursorX - scrollH - 1, cursorY - scrollV, cursorX - scrollH + 1, cursorY + tSize.cy - scrollV);
                        paint.fillRect(TextColor, cRect);
                    }
                }
                else {
                    if (!Focused && text.Length == 0) {
                        if (m_tempText != null && m_tempText.Length > 0) {
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

        /// <summary>
        /// 粘贴方法
        /// </summary>
        public override void onPaste() {
            if (!m_readOnly) {
                FCHost host = Native.Host;
                String insert = host.paste();
                if (insert != null && insert.Length > 0) {
                    int oldLines = m_lines.size();
                    String redotext = Text;
                    insertWord(insert);
                    onTextChanged();
                    if (m_textChanged) {
                        if (redotext != null) {
                            m_undoStack.Push(redotext);
                        }
                    }
                    invalidate();
                    int newLines = m_lines.size();
                    if (newLines != oldLines) {
                        FCVScrollBar vScrollBar = VScrollBar;
                        if (vScrollBar != null) {
                            vScrollBar.Pos += m_lineHeight * (newLines - oldLines);
                            invalidate();
                        }
                    }
                    update();
                    base.onPaste();
                }
            }
        }

        /// <summary>
        /// 被使用TAB切换方法
        /// </summary>
        public override void onTabStop() {
            base.onTabStop();
            if (!m_multiline) {
                if (Text != null) {
                    int textSize = Text.Length;
                    if (textSize > 0) {
                        m_selectionStart = 0;
                        m_selectionLength = textSize;
                        onTimer(m_timerID);
                    }
                }
            }
        }

        /// <summary>
        /// 文字尺寸改变事件
        /// </summary>
        public override void onSizeChanged() {
            base.onSizeChanged();
            if (m_wordWrap) {
                m_textChanged = true;
                invalidate();
            }
        }

        /// <summary>
        /// 文字改变方法
        /// </summary>
        public override void onTextChanged() {
            m_textChanged = true;
            base.onTextChanged();
        }

        /// <summary>
        /// 秒表回调方法
        /// </summary>
        /// <param name="timerID">秒表ID</param>
        public override void onTimer(int timerID) {
            base.onTimer(timerID);
            if (m_timerID == timerID) {
                if (Visible && Focused && !m_textChanged) {
                    m_showCursor = !m_showCursor;
                    invalidate();
                }
            }
        }

        /// <summary>
        /// 重复
        /// </summary>
        /// <returns>重复命令</returns>
        public void redo() {
            if (canRedo()) {
                m_undoStack.Push(Text);
                Text = m_redoStack.Pop();
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        public void selectAll() {
            m_selectionStart = 0;
            if (Text != null) {
                m_selectionLength = Text.Length;
            }
            else {
                m_selectionLength = 0;
            }
        }

        /// <summary>
        /// 设置移动索引
        /// </summary>
        /// <param name="sIndex">开始索引</param>
        /// <param name="eIndex">结束索引</param>
        private void setMovingIndex(int sIndex, int eIndex) {
            int textSize = Text.Length;
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
                m_selectionStart = Math.Min(m_startMovingIndex, m_stopMovingIndex);
                m_selectionLength = Math.Abs(m_startMovingIndex - m_stopMovingIndex);
            }
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "lineheight") {
                LineHeight = FCStr.convertStrToInt(value);
            }
            else if (name == "multiline") {
                Multiline = FCStr.convertStrToBool(value);
            }
            else if (name == "passwordchar") {
                PasswordChar = Convert.ToChar(value);
            }
            else if (name == "readonly") {
                ReadOnly = FCStr.convertStrToBool(value);
            }
            else if (name == "righttoleft") {
                RightToLeft = FCStr.convertStrToBool(value);
            }
            else if (name == "selectionbackcolor") {
                SelectionBackColor = FCStr.convertStrToColor(value);
            }
            else if (name == "selectiontextcolor") {
                SelectionTextColor = FCStr.convertStrToColor(value);
            }
            else if (name == "temptext") {
                TempText = value;
            }
            else if (name == "temptextcolor") {
                TempTextColor = FCStr.convertStrToColor(value);
            }
            else if (name == "textalign") {
                TextAlign = FCStr.convertStrToHorizontalAlign(value);
            }
            else if (name == "wordwrap") {
                WordWrap = FCStr.convertStrToBool(value);
            }
            else {
                base.setProperty(name, value);
            }
        }

        /// <summary>
        /// 撤销
        /// </summary>
        /// <returns>撤销命令</returns>
        public void undo() {
            if (canUndo()) {
                if (Text != null) {
                    m_redoStack.Push(Text);
                }
                Text = m_undoStack.Pop();
            }
        }

        /// <summary>
        /// 更新布局方法
        /// </summary>
        public override void update() {
            FCNative native = Native;
            if (native != null) {
                FCVScrollBar vScrollBar = VScrollBar;
                if (vScrollBar != null) {
                    vScrollBar.LineSize = m_lineHeight;
                }
            }
            base.update();
        }
    }
}
