/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCTEXTBOX_H__
#define __FCTEXTBOX_H__
#pragma once
#include "stdafx.h"
#include "FCHScrollBar.h"
#include "FCVScrollBar.h"
#include "FCDiv.h"
#include <stack>

namespace FaceCat{
    /*
     * 行
     */
    struct WordLine{
    public:
        /**
         * 结束索引
         */
        int m_end;
        /**
         * 开始索引
         */
        int m_start;
        WordLine(int start, int end){
            m_start = start;
            m_end = end;
        }
    };
    
    /*
     * 文本框
     */
    class FCTextBox : public FCDiv{
    private:
        /*
         * TICK值
         */
        int m_tick;
        /**
         * 秒表ID
         */
        int m_timerID;
    protected:
        /**
         * 键盘是否按下
         */
        bool m_isKeyDown;
        /**
         * 是否触摸按下
         */
        bool m_isTouchDown;
        /**
         * 行高
         */
        int m_lineHeight;
        /**
         * 行数
         */
        vector<WordLine> m_lines;
        /**
         * 是否多行显示
         */
        bool m_multiline;
        /**
         * 横向偏移量
         */
        int m_offsetX;
        /**
         * 密码字符
         */
        wchar_t m_passwordChar;
        /**
         * 文字矩形范围
         */
        vector<FCRectF> m_ranges;
        /**
         * 重做栈
         */
        stack<String> m_redoStack;
        /**
         * 是否只读
         */
        bool m_readOnly;
        /**
         * 是否从右向左绘制
         */
        bool m_rightToLeft;
        /**
         * 选中的背景色
         */
        Long m_selectionBackColor;
        /**
         * 选中的前景色
         */
        Long m_selectionTextColor;
        /**
         * 选中长度
         */
        int m_selectionLength;
        /**
         * 选中开始位置
         */
        int m_selectionStart;
        /**
         * 是否显示光标
         */
        bool m_showCursor;
        /**
         * 开始移动的坐标
         */
        int m_startMovingIndex;
        /**
         * 结束移动的坐标
         */
        int m_stopMovingIndex;
        /**
         * 临时文字
         */
        String m_tempText;
        /**
         * 临时文字的颜色
         */
        Long m_tempTextColor;
        /**
         * 内容的横向排列样式
         */
        FCHorizontalAlign m_textAlign;
        /**
         * 文字是否改变
         */
        bool m_textChanged;
        /**
         * 光标闪烁频率
         */
        int TICK;
        /**
         * 撤销栈
         */
        stack<String> m_undoStack;
        /**
         * 文字大小
         */
        vector<FCSizeF> m_wordsSize;
        /**
         * 多行编辑控件是否启动换行
         */
        bool m_wordWrap;
        /**
         * 光标向下移动
         */
        void cursorDown();
        /**
         * 光标移动到最右端
         */
        void cursorEnd();
        /**
         * 光标移动到最左端
         */
        void cursorHome();
        /**
         * 光标向左移动
         */
        void cursorLeft();
        /**
         * 光标向右移动
         */
        void cursorRight();
        /**
         * 光标向上移动
         */
        void cursorUp();
        /**
         * 删除字符
         */
        void deleteWord();
        /**
         * 插入字符
         */
        void insertWord(const String& str);
        /**
         * 判断字符索引所在行是否可见
         */
        bool isLineVisible(int indexTop, int indexBottom, int cell, int floor, int lineHeight, double visiblePercent);
        /*
         * 判断字符索引所在行是否可见
         */
        bool isLineVisible(int index, double visiblePercent);
        /**
         * 设置移动索引
         */
        void setMovingIndex(int sIndex, int eIndex);
    public:
        /*
         * 构造函数
         */
        FCTextBox();
        /*
         * 析构函数
         */
        virtual ~FCTextBox();
        /**
         * 获取行数
         */
        virtual int getLinesCount();
        /**
         * 获取行高
         */
        virtual int getLineHeight();
        /**
         * 设置行高
         */
        virtual void setLineHeight(int lineHeight);
        /**
         * 获取行数
         */
        virtual vector<WordLine> getLines();
        /**
         * 获取是否多行显示
         */
        virtual bool isMultiline();
        /**
         * 设置是否多行显示
         */
        virtual void setMultiline(bool multiline);
        /**
         * 获取密码字符
         */
        virtual wchar_t getPasswordChar();
        /**
         * 设置密码字符
         */
        virtual void setPasswordChar(wchar_t passwordChar);
        /**
         * 获取是否只读
         */
        virtual bool isReadOnly();
        /**
         * 设置是否只读
         */
        virtual void setReadOnly(bool readOnly);
        /**
         * 获取是否从右向左绘制
         */
        virtual bool isRightToLeft();
        /**
         * 设置是否从右向左绘制
         */
        virtual void setRightToLeft(bool rightToLeft);
        /**
         * 获取选中的背景色
         */
        virtual Long getSelectionBackColor();
        /**
         * 设置选中的背景色
         */
        virtual void setSelectionBackColor(Long selectionBackColor);
        /**
         * 获取选中的前景色
         */
        virtual Long getSelectionTextColor();
        /**
         * 设置选中的前景色
         */
        void setSelectionTextColor(Long selectionTextColor);
        /**
         * 获取选中长度
         */
        virtual int getSelectionLength();
        /**
         * 设置选中长度
         */
        virtual void setSelectionLength(int selectionLength);
        /**
         * 获取选中开始位置
         */
        virtual int getSelectionStart();
        /**
         * 设置选中开始位置
         */
        virtual void setSelectionStart(int selectionStart);
        /**
         * 获取临时文字
         */
        virtual String getTempText();
        /**
         * 设置临时文字
         */
        virtual void setTempText(const String& tempText);
        /**
         * 获取临时文字的颜色
         */
        virtual Long getTempTextColor();
        /**
         * 设置临时文字的颜色
         */
        virtual void setTempTextColor(Long tempTextColor);
        /**
         * 获取内容的横向排列样式
         */
        virtual FCHorizontalAlign getTextAlign();
        /**
         * 设置内容的横向排列样式
         */
        virtual void setTextAlign(FCHorizontalAlign textAlign);
        /**
         * 获取多行编辑控件是否启动换行
         */
        virtual bool isWordWrap();
        /**
         * 设置多行编辑控件是否启动换行
         */
        virtual void setWordWrap(bool wordWrap);
    public:
        /**
         * 判断是否可以重复
         */
        bool canRedo();
        /**
         * 判断是否可以撤销
         */
        bool canUndo();
        /**
         * 重置
         */
        void clearRedoUndo();
        /**
         * 获取内容的高度
         */
        virtual int getContentHeight();
        /**
         * 获取内容的宽度
         */
        virtual int getContentWidth();
        /**
         * 获取控件类型
         */
        virtual String getControlType();
        /**
         * 获取属性值
         * @param  name  属性名称
         * @param  value  返回属性值
         * @param  type  返回属性类型
         */
        virtual void getProperty(const String& name, String *value, String *type);
        /**
         * 获取属性名称列表
         */
        virtual ArrayList<String> getPropertyNames();
        /**
         * 获取选中的文字
         */
        String getSelectionText();
        /**
         * 文本输入方法
         */
        virtual void onChar(wchar_t ch);
        /**
         * 复制文字
         */
        virtual void onCopy();
        /**
         * 剪切
         */
        virtual void onCut();
        /**
         * 获取焦点方法
         */
        virtual void onGotFocus();
        /**
         * 键盘方法
         */
        virtual void onKeyDown(char key);
        /**
         * 键盘抬起方法
         */
        virtual void onKeyUp(char key);
        /**
         * 丢失焦点方法
         */
        virtual void onLostFocus();
        /**
         * 触摸按下方法
         */
        virtual void onTouchDown(FCTouchInfo touchInfo);
        /**
         * 触摸移动方法
         */
        virtual void onTouchMove(FCTouchInfo touchInfo);
        /**
         * 触摸抬起方法
         */
        virtual void onTouchUp(FCTouchInfo touchInfo);
        /**
         * 重绘前景方法
         */
        virtual void onPaintForeground(FCPaint *paint, const FCRect& clipRect);
        /**
         * 粘贴方法
         */
        virtual void onPaste();
        /**
         * 文字尺寸改变事件
         */
        virtual void onSizeChanged();
        /**
         * 被使用TAB切换方法
         */
        virtual void onTabStop();
        /**
         * 文字改变方法
         */
        virtual void onTextChanged();
        /**
         * 秒表回调方法
         */
        virtual void onTimer(int timerID);
        /**
         * 重复
         */
        void redo();
        /**
         * 全选
         */
        void selectAll();
        /**
         * 设置属性
         * @param  name  属性名称
         * @param  value  属性值
         */
        virtual void setProperty(const String& name, const String& value);
        /**
         * 撤销
         */
        void undo();
        /**
         * 更新布局方法
         */
        virtual void update();
    };
}

#endif
