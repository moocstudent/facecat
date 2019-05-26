/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCCHECKBOX_H__
#define __FCCHECKBOX_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\core\\FCStr.h"
#include "FCButton.h"

namespace FaceCat{
	/**
     * 复选框控件
     */
	class FCCheckBox : public FCButton{
	protected:
		/**
	     * 获取或设置内容的横向排列样式
	     */
		FCHorizontalAlign m_buttonAlign;
		/**
	     * 获取或设置按钮的背景色
	     */
		Long m_buttonBackColor;
		/**
	     * 获取或设置按钮的边线颜色
	     */
		Long m_buttonBorderColor;
		/**
	     * 获取或设置按钮的尺寸
	     */
		FCSize m_buttonSize;
		/**
	     * 获取或设置是否被选中
	     */
		bool m_checked;
		/**
	     * 获取或设置选中时的背景图
	     */
		String m_checkedBackImage;
		/**
	     * 获取或设置触摸悬停且选中时的背景图片
	     */
		String m_checkHoveredBackImage;
		/**
	     * 获取或设置触摸按下且选中时的背景图片
	     */
		String m_checkPushedBackImage;
		/**
	     * 获取或设置不可用时的选中背景图片
	     */
		String m_disableCheckedBackImage;
	protected:
	    /**
	     * 获取或设置的背景色
	     */
		virtual Long getPaintingBackColor();
		/**
	     * 获取要绘制的按钮的背景色
	     */
		virtual Long getPaintingButtonBackColor();
		/**
	     * 获取要绘制的按钮边线颜色
	     */
		virtual Long getPaintingButtonBorderColor();
		/**
	     * 获取用于绘制的背景图片
	     */
		virtual String getPaintingBackImage();
	public:
		/*
		* 构造函数
		*/
		FCCheckBox();
		/*
		* 析构函数
		*/
		virtual ~FCCheckBox();
		/**
	     * 获取内容的横向排列样式
	     */
		virtual FCHorizontalAlign getButtonAlign();
		/**
	     * 设置内容的横向排列样式
	     */
		virtual void setButtonAlign(FCHorizontalAlign buttonAlign);
		/**
	     * 获取按钮的背景色
	     */
		virtual Long getButtonBackColor();
		/**
	     * 设置按钮的背景色
	     */
		virtual void setButtonBackColor(Long buttonBackColor);
		/**
	     * 获取按钮的边线颜色
	     */
		virtual Long getButtonBorderColor();
		/**
	     * 设置按钮的边线颜色
	     */
		virtual void setButtonBorderColor(Long buttonBorderColor);
		/**
	     * 获取按钮的尺寸
	     */
		virtual FCSize getButtonSize();
		/**
	     * 设置按钮的尺寸
	     */
		virtual void setButtonSize(FCSize buttonSize);
		/**
	     * 获取是否被选中
	     */
		virtual bool isChecked();
		/**
	     * 设置是否被选中
	     */
		virtual void setChecked(bool checked);
		/**
	     * 获取选中时的背景图
	     */
		virtual String getCheckedBackImage();
		/**
	     * 设置选中时的背景图
	     */
		virtual void setCheckedBackImage(const String& checkedBackImage);
		/**
	     * 获取触摸悬停且选中时的背景图片
	     */
		virtual String getCheckHoveredBackImage();
		/**
	     * 设置触摸悬停且选中时的背景图片
	     */
		virtual void setCheckHoveredBackImage(const String& checkHoveredBackImage);
		/**
	     * 获取触摸按下且选中时的背景图片
	     */
		virtual String getCheckPushedBackImage();
		/**
	     * 设置触摸按下且选中时的背景图片
	     */
		virtual void setCheckPushedBackImage(const String& checkPushedBackImage);
		/**
	     * 获取不可用时的选中背景图片
	     */
		virtual String getDisableCheckedBackImage();
		/**
	     * 设置不可用时的选中背景图片
	     */
		virtual void setDisableCheckedBackImage(const String& disableCheckedBackImage);
	public:
	    /**
	     * 获取控件类型
	     */
		virtual String getControlType();
        /**
          * 获取属性值
          * @param name  属性名称
          * @param value 返回属性值
          * @param type  返回属性类型
         */
		virtual void getProperty(const String& name, String *value, String *type);
		/**
	      * 获取属性名称列表
	     */
		virtual ArrayList<String> getPropertyNames();
		/**
	      * 选中改变方法
	     */
		virtual void onCheckedChanged();
	    /**
         * 点击方法
         * @param touchInfo 触摸信息
        */
		virtual void onClick(FCTouchInfo touchInfo);
	    /**
         * 重绘背景方法
         * @param paint    绘图对象
         * @param clipRect 裁剪区域
        */
		virtual void onPaintBackground(FCPaint *paint, const FCRect& clipRect);
	    /**
         * 重绘选中按钮方法
         * @param paint    绘图对象
         * @param clipRect 裁剪区域
        */
		virtual void onPaintCheckButton(FCPaint *paint, const FCRect& clipRect);
	    /**
         * 重绘前景方法
         * @param paint    绘图对象
         * @param clipRect 裁剪区域
        */
		virtual void onPaintForeground(FCPaint *paint, const FCRect& clipRect);
	    /**
         * 设置属性
         * @param name    属性名称
         * @param value   属性值
        */
		virtual void setProperty(const String& name, const String& value);
	};
}

#endif