/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCCOMBOBOX_H__
#define __FCCOMBOBOX_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "FCTextBox.h"
#include "..\\btn\\FCButton.h"
#include "..\\div\\FCMenu.h"
#include "..\\div\\FCMenuItem.h"

namespace FaceCat{
	class FCComboBox;

	/*
	* 下拉列表的菜单
	*/
	class FCComboBoxMenu : public FCMenu{
	protected:
	    /**
		 * 列表控件
		 */
		FCComboBox *m_comboBox;
	public:
		/*
		* 构造函数
		*/
		FCComboBoxMenu();
		/*
		* 析构函数
		*/
		virtual ~FCComboBoxMenu();
		/**
		 * 获取列表控件
		 */
		FCComboBox* getComboBox();
		/**
		 * 设置列表控件
		 */
		void setComboBox(FCComboBox *comboBox);
		/**
		 * 是否自动隐藏
		 */
		virtual bool onAutoHide();
	};

	/*
	* 下拉列表
	*/
	class FCComboBox : public FCTextBox{
	protected:
		/*
		* 下拉按钮
		*/
		FCButton* m_dropDownButton;
		/**
		 * 下拉按钮的点击函数
		 */
		FCTouchEvent m_dropDownButtonTouchDownEvent;
		/**
		 * 下拉菜单的点击函数
		 */
		FCMenuItemTouchEvent m_menuItemClickEvent;
		/**
		 * 下拉菜单的按键函数
		 */
		FCKeyEvent m_menuKeyDownEvent;
		/*
		* 下拉菜单
		*/
		FCComboBoxMenu* m_dropDownMenu;
		/**
		 * 下拉按钮的点击方法
		 */
		static void dropDownButtonTouchDown(Object sender, FCTouchInfo touchInfo, Object pInvoke);
		/**
		 * 菜单项的点击方法
		 */
		static void menuItemClick(Object sender, FCMenuItem *item, FCTouchInfo touchInfo, Object pInvoke);
		/**
		 * 下拉菜单的按键方法
		 */
		static void menuKeyDown(Object sender, char key, Object pInvoke);
	public:
		/*
		* 构造函数
		*/
		FCComboBox();
		/*
		* 析构函数
		*/
		virtual ~FCComboBox();
		/**
		 * 获取下拉按钮
		 */
		virtual FCButton* getDropDownButton();
		/**
		 * 获取下拉菜单
		 */
		virtual FCComboBoxMenu* getDropDownMenu();
		/**
		 * 获取选中的索引
		 */
		virtual int getSelectedIndex();
		/**
		 * 设置选中的索引
		 */
		virtual void setSelectedIndex(int selectedIndex);
		/**
		 * 获取选中的文字
		 */
		virtual String getSelectedText();
		/**
		 * 设置选中的文字
		 */
		virtual void setSelectedText(const String& selectedText);
		/**
		 * 获取选中的值
		 */
		virtual String getSelectedValue();
		/**
		 * 设置选中的值
		 */
		virtual void setSelectedValue(const String& selectedValue);
	public:
	    /**
		 * 添加菜单项
		 */
		void addItem(FCMenuItem *item);
		/**
		 * 清除所有菜单项
		 */
		void clearItems();
		/**
		 * 获取控件类型
		 */
		virtual String getControlType();
		/**
		 * 获取菜单项
		 */
		ArrayList<FCMenuItem*> getItems();
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
		 * 插入菜单项
		 */
		void insertItem(int index, FCMenuItem *item);
		/**
		 * 菜单下拉方法
		 */
		virtual void onDropDownOpening();
		/**
		 * 键盘按下方法
		 */
		virtual void onKeyDown(char key);
		/**
		 * 添加控件方法
		 */
		virtual void onLoad();
		/**
		 * 选中索引改变方法
		 */
		virtual void onSelectedIndexChanged();
		/**
		 * 触摸滚轮方法
		 */
		virtual void onTouchWheel(FCTouchInfo touchInfo);
		/**
		 * 移除菜单项
		 */
		void removeItem(FCMenuItem *item);
		/**
		* 设置属性
		* @param  name  属性名称
		* @param  value  属性值
		*/
		virtual void setProperty(const String& name, const String& value);
		/**
		 * 更新布局方法
		 */
		virtual void update();
	};
}

#endif