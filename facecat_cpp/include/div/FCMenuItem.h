/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCMENUITEM_H__
#define __FCMENUITEM_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\btn\\FCButton.h"
#include "FCMenu.h"

namespace FaceCat{
	class FCMenu;

	/*
	* 菜单项
	*/
	class FCMenuItem : public FCButton{
	protected:
	    /**
		 * 是否选中
		 */
		bool m_checked;
		/**
		 * 下拉表格
		 */
		FCMenu *m_dropDownMenu;
		/**
		 * 父菜单项
		 */
		FCMenuItem *m_parentItem;
		/**
		 * 所在菜单
		 */
		FCMenu *m_parentMenu;
		/**
		 * 值
		 */
		String m_value;
	public:
	    /**
		 * 所有菜单项
		 */
		ArrayList<FCMenuItem*> m_items;
		/*
		* 构造函数
		*/
		FCMenuItem();
		/*
		* 构造函数
		*/
		FCMenuItem(const String& text);
		/*
		* 析构函数
		*/
		virtual ~FCMenuItem();
		/**
		 * 获取是否选中
		 */
		bool isChecked();
		/**
		 * 设置是否选中
		 */
		void setChecked(bool checked);
		/**
		 * 获取下拉表格
		 */
		FCMenu* getDropDownMenu();
		/**
		 * 设置下拉表格
		 */
		void setDropDownMenu(FCMenu *dropDownMenu);
		/**
		 * 获取父菜单项
		 */
		FCMenuItem* getParentItem();
		/**
		 * 设置父菜单项
		 */
		void setParentItem(FCMenuItem *parentItem);
		/**
		 * 获取置所在菜单
		 */
		FCMenu* getParentMenu();
		/**
		 * 设置所在菜单
		 */
		void setParentMenu(FCMenu *parentMenu);
		/**
		 * 获取值
		 */
		String getValue();
		/**
		 * 设置值
		 */
		void setValue(const String& value);
	public:
		/**
		* 添加子菜单项
		* @param  item  菜单项
		*/
		void addItem(FCMenuItem *item);
		/**
		 * 清除所有的项
		 */
		void clearItems();
		/**
		 * 获取控件类型
		 */
		virtual String getControlType();
		/**
		 * 获取所有的项
		 */
		ArrayList<FCMenuItem*> getItems();
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
		* 插入项
		* @param  index  索引
		* @param  item  菜单项
		*/
		void insertItem(int index, FCMenuItem *item);
		/**
		* 添加项
		* @param  index  行索引
		*/
		virtual void onAddingItem(int index);
		/**
		* 点击方法
		* @param  touchInfo  触摸信息
		*/
		virtual void onClick(FCTouchInfo touchInfo);
		/**
		* 触摸移动方法
		* @param  touchInfo  触摸信息
		*/
		virtual void onTouchMove(FCTouchInfo touchInfo);
		/**
		* 重绘前景方法
		* @param  paint  绘图对象
		* @param  clipRect  裁剪矩形
		*/
		virtual void onPaintForeground(FCPaint *paint, const FCRect& clipRect);
		/**
		 * 移除项方法
		 */
		virtual void onRemovingItem();
		/**
		* 移除子菜单项
		* @param  item   子菜单项
		*/
		void removeItem(FCMenuItem *item);
		/**
		* 设置属性
		* @param  name   属性名
		* @param  value   属性值
		*/
		virtual void setProperty(const String& name, const String& value);
	};
}

#endif