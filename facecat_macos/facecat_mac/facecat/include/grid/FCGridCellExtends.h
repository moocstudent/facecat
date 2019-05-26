/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCGRIDCELLEXTENDS_H__
#define __FCGRIDCELLEXTENDS_H__
#pragma once
#include "stdafx.h"
#include "FCGridCell.h"
#include "FCGridColumn.h"
#include "FCGridRow.h"
#include "FCGrid.h"

namespace FaceCat{
    class FCGridCell;
    class FCGridColumn;
    class FCGridRow;
    class FCGrid;
    class FCButton;
    class FCCheckBox;
    class FCComboBox;
    class FCDateTimePicker;
    class FCLabel;
    class FCSpin;
    class FCRadioButton;
    class FCTextBox;
    
    /*
     * 布尔型单元格
     */
    class FCGridBoolCell : public FCGridCell{
    protected:
        /**
         * 数值
         */
        bool m_value;
    public:
        FCGridBoolCell();
        FCGridBoolCell(bool value);
        virtual ~FCGridBoolCell();
    public:
        /**
         * 单元格大小比较，用于排序
         */
        virtual int compareTo(FCGridCell *cell);
        /**
         * 获取布尔型数值
         */
        virtual bool getBool();
        /**
         * 获取双精度浮点值
         */
        virtual double getDouble();
        /**
         * 获取单精度浮点值
         */
        virtual float getFloat();
        /**
         * 获取整型数值
         */
        virtual int getInt();
        /**
         * 获取长整型数值
         */
        virtual Long getLong();
        /**
         * 获取字符型数值
         */
        virtual String getString();
        /**
         * 设置布尔型数值
         */
        virtual void setBool(bool value);
        /**
         * 设置双精度浮点值
         */
        virtual void setDouble(double value);
        /**
         * 设置单精度浮点值
         */
        virtual void setFloat(float value);
        /**
         * 设置整型数值
         */
        virtual void setInt(int value);
        /**
         * 设置长整型数值
         */
        virtual void setLong(Long value);
        /**
         * 设置字符型数值
         */
        virtual void setString(const String& value);
    };
    
    /*
     * 按钮型单元格
     */
    class FCGridButtonCell: public FCGridControlCell{
    public:
        FCGridButtonCell();
        virtual ~FCGridButtonCell();
        FCButton* getButton();
    };
    
    /*
     * 复选框型单元格
     */
    class FCGridCheckBoxCell : public FCGridControlCell{
    protected:
    public:
        FCGridCheckBoxCell();
        virtual ~FCGridCheckBoxCell();
        FCCheckBox* getCheckBox();
    public:
        /**
         * 获取布尔型数值
         */
        virtual bool getBool();
        /**
         * 获取双精度浮点值
         */
        virtual double getDouble();
        /**
         * 获取单精度浮点值
         */
        virtual float getFloat();
        /**
         * 获取整型数值
         */
        virtual int getInt();
        /**
         * 获取长整型数值
         */
        virtual Long getLong();
        /**
         * 获取布尔型数值
         */
        virtual String getString();
        /**
         * 设置布尔型数值
         */
        virtual void setBool(bool value);
        /**
         * 设置双精度浮点值
         */
        virtual void setDouble(double value);
        /**
         * 设置单精度浮点值
         */
        virtual void setFloat(float value);
        /**
         * 设置整型数值
         */
        virtual void setInt(int value);
        /**
         * 设置长整型数值
         */
        virtual void setLong(Long value);
        /**
         * 设置字符型数值
         */
        virtual void setString(const String& value);
    };
    
    /*
     * 下拉列表单元格
     */
    class FCGridComboBoxCell : public FCGridControlCell{
    public:
        FCGridComboBoxCell();
        virtual ~FCGridComboBoxCell();
        FCComboBox* getComboBox();
    public:
        virtual bool getBool();
        virtual double getDouble();
        virtual float getFloat();
        virtual int getInt();
        virtual Long getLong();
        virtual String getString();
        virtual void setBool(bool value);
        virtual void setDouble(double value);
        virtual void setFloat(float value);
        virtual void setInt(int value);
        virtual void setLong(Long value);
        virtual void setString(const String& value);
    };
    
    /*
     * 日期时间选择单元格
     */
    class FCGridDateTimePickerCell : public FCGridControlCell{
    public:
        FCGridDateTimePickerCell();
        virtual ~FCGridDateTimePickerCell();
        FCDateTimePicker* getDatePicker();
    };
    
    /*
     * 图层单元格
     */
    class FCGridDivCell : public FCGridControlCell{
    public:
        FCGridDivCell();
        virtual ~FCGridDivCell();
        FCDiv* getDiv();
    };
    
    /*
     * 双精度单元格
     */
    class FCGridDoubleCell : public FCGridCell{
    protected:
        double m_value;
    public:
        FCGridDoubleCell();
        FCGridDoubleCell(double value);
        virtual ~FCGridDoubleCell();
    public:
        virtual int compareTo(FCGridCell *cell);
        virtual bool getBool();
        virtual double getDouble();
        virtual float getFloat();
        virtual int getInt();
        virtual Long getLong();
        virtual String getString();
        virtual void setBool(bool value);
        virtual void setDouble(double value);
        virtual void setFloat(float value);
        virtual void setInt(int value);
        virtual void setLong(Long value);
        virtual void setString(const String& value);
    };
    
    /*
     * 单精度单元格
     */
    class FCGridFloatCell : public FCGridCell{
    protected:
        float m_value;
    public:
        FCGridFloatCell();
        FCGridFloatCell(float value);
        virtual ~FCGridFloatCell();
    public:
        virtual int compareTo(FCGridCell *cell);
        virtual bool getBool();
        virtual double getDouble();
        virtual float getFloat();
        virtual int getInt();
        virtual Long getLong();
        virtual String getString();
        virtual void setBool(bool value);
        virtual void setDouble(double value);
        virtual void setFloat(float value);
        virtual void setInt(int value);
        virtual void setLong(Long value);
        virtual void setString(const String& value);
    };
    
    /*
     * 整型单元格
     */
    class FCGridIntCell : public FCGridCell{
    protected:
        int m_value;
    public:
        FCGridIntCell();
        FCGridIntCell(int value);
        virtual ~FCGridIntCell();
    public:
        virtual int compareTo(FCGridCell *cell);
        virtual bool getBool();
        virtual double getDouble();
        virtual float getFloat();
        virtual int getInt();
        virtual Long getLong();
        virtual String getString();
        virtual void setBool(bool value);
        virtual void setDouble(double value);
        virtual void setFloat(float value);
        virtual void setInt(int value);
        virtual void setLong(Long value);
        virtual void setString(const String& value);
    };
    
    /*
     * 标签单元格
     */
    class FCGridLabelCell : public FCGridControlCell{
    public:
        FCGridLabelCell();
        virtual ~FCGridLabelCell();
        FCLabel* getLabel();
    };
    
    /*
     * 长整型单元格
     */
    class FCGridLongCell : public FCGridCell{
    protected:
        Long m_value;
    public:
        FCGridLongCell();
        FCGridLongCell(Long value);
        virtual ~FCGridLongCell();
    public:
        virtual int compareTo(FCGridCell *cell);
        virtual bool getBool();
        virtual double getDouble();
        virtual float getFloat();
        virtual int getInt();
        virtual Long getLong();
        virtual String getString();
        virtual void setBool(bool value);
        virtual void setDouble(double value);
        virtual void setFloat(float value);
        virtual void setInt(int value);
        virtual void setLong(Long value);
        virtual void setString(const String& value);
    };
    
    /*
     * 单选框单元格
     */
    class FCGridRadioButtonCell : public FCGridControlCell{
    public:
        FCGridRadioButtonCell();
        virtual ~FCGridRadioButtonCell();
        FCRadioButton* getRadioButton();
    public:
        virtual bool getBool();
        virtual double getDouble();
        virtual float getFloat();
        virtual int getInt();
        virtual Long getLong();
        virtual String getString();
        virtual void setBool(bool value);
        virtual void setDouble(double value);
        virtual void setFloat(float value);
        virtual void setInt(int value);
        virtual void setLong(Long value);
        virtual void setString(const String& value);
    };
    
    /*
     * 数值文本框单元格
     */
    class FCGridSpinCell : public FCGridControlCell{
    public:
        FCGridSpinCell();
        virtual ~FCGridSpinCell();
        FCSpin* getSpin();
    public:
        virtual bool getBool();
        virtual double getDouble();
        virtual float getFloat();
        virtual int getInt();
        virtual Long getLong();
        virtual void setBool(bool value);
        virtual void setDouble(double value);
        virtual void setFloat(float value);
        virtual void setInt(int value);
        virtual void setLong(Long value);
    };
    
    /*
     * 字符串单元格
     */
    class FCGridStringCell : public FCGridCell{
    protected:
        String m_value;
    public:
        FCGridStringCell();
        FCGridStringCell(const String& value);
        virtual ~FCGridStringCell();
    public:
        virtual int compareTo(FCGridCell *cell);
        virtual bool getBool();
        virtual double getDouble();
        virtual float getFloat();
        virtual int getInt();
        virtual Long getLong();
        virtual String getString();
        virtual void setString(const String& value);
    };
    
    /*
     * 密码单元格
     */
    class FCGridPasswordCell : public FCGridStringCell{
    public:
        FCGridPasswordCell();
        virtual ~FCGridPasswordCell();
    public:
        virtual String getPaintText();
    };
    
    /*
     * 文本框单元格
     */
    class FCGridTextBoxCell: public FCGridControlCell{
    public:
        FCGridTextBoxCell();
        virtual ~FCGridTextBoxCell();
        FCTextBox* getTextBox();
    };
}
#endif
