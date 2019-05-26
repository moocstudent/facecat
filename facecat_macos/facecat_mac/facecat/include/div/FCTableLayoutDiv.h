/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCTABLELAYOUTDIV_H__
#define __FCTABLELAYOUTDIV_H__
#pragma once
#include "stdafx.h"
#include "FCDiv.h"

namespace FaceCat{
    /**
     * 调整大小的类型
     */
    enum FCSizeType{
        /**
         * 绝对大小
         */
        FCSizeType_AbsoluteSize,
        /**
         * 自动填充
         */
        FCSizeType_AutoFill,
        /**
         * 百分比大小
         */
        FCSizeType_PercentSize
    };
    
    /*
     * 列的样式
     */
    class FCColumnStyle : public FCProperty{
    protected:
        /**
         * 调整大小的类型
         */
        FCSizeType m_sizeType;
        /**
         * 宽度
         */
        float m_width;
    public:
        /*
         * 构造函数
         */
        FCColumnStyle();
        /*
         * 构造函数
         */
        FCColumnStyle(FCSizeType sizeType, float width);
        /*
         * 析构函数
         */
        virtual ~FCColumnStyle();
        /**
         * 获取调整大小的类型
         */
        virtual FCSizeType getSizeType();
        /**
         * 设置调整大小的类型
         */
        virtual void setSizeTypeA(FCSizeType  sizeType);
        /**
         * 获取宽度
         */
        virtual float getWidth();
        /**
         * 设置宽度
         */
        virtual void setWidth(float width);
    public:
        /**
         * 获取属性值
         * @param  name   属性名称
         * @param  value   返回属性值
         * @param  type   返回属性类型
         */
        virtual void getProperty(const String& name, String *value, String *type);
        /**
         * 获取属性名称列表
         */
        virtual ArrayList<String> getPropertyNames();
        /**
         * 设置属性值
         * @param  name   属性名称
         * @param  value   属性值
         */
        virtual void setProperty(const String& name, const String& value);
    };
    
    /*
     * 行的央视u
     */
    class FCRowStyle : public FCProperty{
    protected:
        /**
         * 宽度
         */
        float m_height;
        /**
         * 调整大小的类型
         */
        FCSizeType m_sizeType;
    public:
        /*
         * 构造函数
         */
        FCRowStyle();
        /*
         * 构造函数
         */
        FCRowStyle(FCSizeType sizeType, float height);
        /*
         * 析构函数
         */
        virtual ~FCRowStyle();
        /**
         * 获取宽度
         */
        virtual float getHeight();
        /**
         * 设置宽度
         */
        virtual void setHeight(float height);
        /**
         * 获取调整大小的类型
         */
        virtual FCSizeType getSizeType();
        /**
         * 设置调整大小的类型
         */
        virtual void setSizeTypeA(FCSizeType  sizeType);
    public:
        /**
         * 获取属性值
         * @param  name   属性名称
         * @param  value   返回属性值
         * @param  type   返回属性类型
         */
        virtual void getProperty(const String& name, String *value, String *type);
        /**
         * 获取属性名称列表
         */
        virtual ArrayList<String> getPropertyNames();
        /**
         * 设置属性值
         * @param  name   属性名称
         * @param  value   属性值
         */
        virtual void setProperty(const String& name, const String& value);
    };
    
    /*
     * 表格层
     */
    class FCTableLayoutDiv : public FCDiv{
    protected:
        /**
         * 列的集合
         */
        ArrayList<int> m_columns;
        /**
         * 列的数量
         */
        int m_columnsCount;
        /**
         * 行的集合
         */
        ArrayList<int> m_rows;
        /**
         * 行的数量
         */
        int m_rowsCount;
        /**
         * 表格控件
         */
        ArrayList<FCView*> m_tableControls;
    public:
        /**
         * 列的样式
         */
        ArrayList<FCColumnStyle> m_columnStyles;
        /**
         * 行的样式
         */
        ArrayList<FCRowStyle> m_rowStyles;
        /*
         * 构造函数
         */
        FCTableLayoutDiv();
        /*
         * 析构函数
         */
        virtual ~FCTableLayoutDiv();
        /**
         * 获取列的数量
         */
        virtual int getColumnsCount();
        /**
         * 设置列的数量
         */
        virtual void setColumnsCount(int columnsCount);
        /**
         * 获取行的数量
         */
        virtual int getRowsCount();
        /**
         * 设置行的数量
         */
        virtual void setRowsCount(int rowsCount);
    public:
        /**
         * 添加控件
         * @param  control   控件
         */
        virtual void addControl(FCView *control);
        /**
         * 添加控件
         * @param  control   控件
         * @param  column   列
         * @param  row   行
         */
        virtual void addControl(FCView *control, int column, int row);
        /**
         * 获取控件类型
         */
        virtual String getControlType();
        /**
         * 获取属性值
         * @param  name   属性名称
         * @param  value   返回属性值
         * @param  type   返回属性类型
         */
        virtual void getProperty(const String& name, String *value, String *type);
        /**
         * 获取属性名称列表
         */
        virtual ArrayList<String> getPropertyNames();
        /**
         * 重置布局
         */
        virtual bool onResetLayout();
        /**
         * 移除控件
         * @param  control   控件
         */
        virtual void removeControl(FCView *control);
        /**
         * 设置属性值
         * @param  name   属性名称
         * @param  value   属性值
         */
        virtual void setProperty(const String& name, const String& value);
        /**
         * 布局更新方法
         */
        virtual void update();
    };
}

#endif
