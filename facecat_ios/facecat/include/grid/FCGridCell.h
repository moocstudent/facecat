/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCGRIDCELL_H__
#define __FCGRIDCELL_H__
#pragma once
#include "stdafx.h"
#include "FCProperty.h"
#include "FCGridColumn.h"
#include "FCGridRow.h"
#include "FCGrid.h"

namespace FaceCat{
    class FCGridColumn;
    class FCGridRow;
    class FCGrid;
    
    /*
     * 单元格样式
     */
    class FCGridCellStyle{
    protected:
        FCHorizontalAlign m_align;
        /**
         * 是否在文字超出范围时在结尾显示省略号
         */
        bool m_autoEllipsis;
        /**
         * 获取
         */
        Long m_backColor;
        /**
         * 字体
         */
        FCFont *m_font;
        /**
         * 前景色
         */
        Long m_textColor;
    public:
        /*
         * 构造函数
         */
        FCGridCellStyle();
        /*
         * 析构函数
         */
        virtual ~FCGridCellStyle();
        /**
         * 获取内容的横向排列样式
         */
        virtual FCHorizontalAlign getAlign();
        /**
         * 设置内容的横向排列样式
         */
        virtual void setAlign(FCHorizontalAlign align);
        /**
         * 获取是否在文字超出范围时在结尾显示省略号
         */
        virtual bool autoEllipsis();
        /**
         * 设置是否在文字超出范围时在结尾显示省略号
         */
        virtual void setAutoEllipsis(bool autoEllipsis);
        /*
         * 获取背景色
         */
        virtual Long getBackColor();
        /*
         * 设置背景色
         */
        virtual void setBackColor(Long backColor);
        /**
         * 获取字体
         */
        virtual FCFont* getFont();
        /**
         * 设置字体
         */
        virtual void setFont(FCFont *font);
        /**
         * 获取前景色
         */
        virtual Long getTextColor();
        /**
         * 设置前景色
         */
        virtual void setTextColor(Long textColor);
    public:
        /*
         * 拷贝
         */
        void copy(FCGridCellStyle *style);
    };
    
    /*
     * 单元格
     */
    class FCGridCell : public FCProperty{
    protected:
        /**
         * 是否可编辑
         */
        bool m_allowEdit;
        /*
         * 列的跨度
         */
        int m_colSpan;
        /**
         * 所在列
         */
        FCGridColumn *m_column;
        /**
         * 表格
         */
        FCGrid *m_grid;
        /**
         * 名称
         */
        String m_name;
        /**
         * 所在行
         */
        FCGridRow *m_row;
        /*
         * 行的跨度
         */
        int m_rowSpan;
        /**
         * 样式
         */
        FCGridCellStyle *m_style;
        /**
         * TAG值
         */
        Object m_tag;
    public:
        /*
         * 构造函数
         */
        FCGridCell();
        /*
         * 析构函数
         */
        virtual ~FCGridCell();
        /**
         * 获取是否可编辑
         */
        virtual bool allowEdit();
        /**
         * 设置是否可编辑
         */
        virtual void setAllowEdit(bool allowEdit);
        /**
         * 获取跨越的列数
         */
        virtual int getColSpan();
        /**
         *设置跨越的列数
         */
        virtual void setColSpan(int colSpan);
        /**
         * 获取所在列
         */
        virtual FCGridColumn* getColumn();
        /**
         * 设置所在列
         */
        virtual void setColumn(FCGridColumn *column);
        /**
         * 获取表格
         */
        virtual FCGrid* getGrid();
        /**
         * 设置表格
         */
        virtual void setGrid(FCGrid *grid);
        /**
         * 获取名称
         */
        virtual String getName();
        /**
         * 设置名称
         */
        virtual void setName(const String& name);
        /**
         * 获取所在行
         */
        virtual FCGridRow* getRow();
        /**
         * 设置所在行
         */
        virtual void setRow(FCGridRow *row);
        /**
         * 获取跨越的行数
         */
        virtual int getRowSpan();
        /**
         * 设置跨越的行数
         */
        virtual void setRowSpan(int rowSpan);
        /**
         * 获取样式
         */
        virtual FCGridCellStyle* getStyle();
        /**
         * 设置样式
         */
        virtual void setStyle(FCGridCellStyle *style);
        /**
         * 获取TAG值
         */
        virtual Object getTag();
        /**
         * 设置TAG值
         */
        virtual void setTag(Object tag);
        /**
         * 获取文字
         */
        virtual String getText();
        /**
         * 设置文字
         */
        virtual void setText(const String& text);
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
         * 获取要绘制的文字
         */
        virtual String getPaintText();
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
         * 获取字符型数值
         */
        virtual String getString();
        /**
         * 添加单元格方法
         */
        virtual void onAdd();
        /**
         * 重绘方法
         * @param  paint  绘图对象
         * @param  rect  矩形
         * @param  clipRect  裁剪矩形
         * @param  isAlternate  是否交替行
         */
        virtual void onPaint(FCPaint *paint, const FCRect& rect, const FCRect& clipRect, bool isAlternate);
        /**
         * 移除单元格方法
         */
        virtual void onRemove();
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
         * 设置属性
         * @param  name  属性名称
         * @param  value  属性值
         */
        virtual void setProperty(const String& name, const String& value);
        /**
         * 设置字符型数值
         */
        virtual void setString(const String& value);
    };
    
    /*
     * 控件型单元格
     */
    class FCGridControlCell : public FCGridCell{
    protected:
        FCView *m_control;
        /**
         * 触摸按下事件
         */
        FCTouchEvent m_touchDownEvent;
        /**
         * 触摸移动事件
         */
        FCTouchEvent m_touchMoveEvent;
        /**
         * 触摸抬起事件
         */
        FCTouchEvent m_touchUpEvent;
        /**
         * 触摸滚轮事件
         */
        FCTouchEvent m_touchWheelEvent;
    protected:
        /**
         * 控件触摸按下事件
         * @param  sender  调用者
         * @param  touchInfo    触摸信息
         */
        static void controlTouchDown(Object sender, FCTouchInfo touchInfo, Object pInvoke);
        /**
         * 控件触摸移动事件
         * @param  sender  调用者
         */
        static void controlTouchMove(Object sender, FCTouchInfo touchInfo, Object pInvoke);
        /**
         * 控件触摸抬起事件
         * @param  sender  调用者
         */
        static void controlTouchUp(Object sender, FCTouchInfo touchInfo, Object pInvoke);
        /**
         * 控件触摸滚轮滚动事件
         * @param  sender  调用者
         * @param  touchInfo   触摸信息
         */
        static void controlTouchWheel(Object sender, FCTouchInfo touchInfo, Object pInvoke);
    public:
        /*
         * 构造函数
         */
        FCGridControlCell();
        /*
         * 析构函数
         */
        virtual ~FCGridControlCell();
        /**
         * 获取控件
         */
        FCView* getControl();
        /**
         * 设置控件
         */
        void setControl(FCView *control);
        /**
         * 获取字符型数值
         */
        virtual String getString();
        /**
         * 获取要绘制的文字
         */
        virtual String getPaintText();
        /**
         * 添加单元格方法
         */
        virtual void onAdd();
        /**
         * 控件触摸按下方法
         */
        virtual void onControlTouchDown(FCTouchInfo touchInfo);
        /**
         * 控件触摸移动事件
         */
        virtual void onControlTouchMove(FCTouchInfo touchInfo);
        /**
         * 控件触摸抬起事件
         */
        virtual void onControlTouchUp(FCTouchInfo touchInfo);
        /**
         * 控件触摸滚轮滚动事件
         */
        virtual void onControlTouchWheel(FCTouchInfo touchInfo);
        /**
         * 重绘方法
         */
        virtual void onPaint(FCPaint *paint, const FCRect& rect, const FCRect& clipRect, bool isAlternate);
        /*
         * 重绘控件
         */
        virtual void onPaintControl(FCPaint *paint, const FCRect& rect, const FCRect& clipRect);
        /**
         * 移除单元格方法
         */
        virtual void onRemove();
        /**
         * 设置字符型数值s
         */
        virtual void setString(const String& value);
    };
}

#endif
