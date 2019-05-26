/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCMENU_H__
#define __FCMENU_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\div\\FCLayoutDiv.h"
#include "FCMenuItem.h"

namespace FaceCat{
	class FCMenuItem;
	/**
	* 点击菜单事件
    */
	typedef void (*FCMenuItemTouchEvent)(Object, FCMenuItem*, FCTouchInfo, Object);

	/*
	* 菜单
	*/
	class FCMenu : public FCLayoutDiv{
	private:
	    /**
		 * 秒表编号
		 */
		int m_timerID;
	protected:
	    /**
		 * 是否自动隐藏
		 */
		bool m_autoHide;
		/**
		 * 父菜单项
		 */
		FCMenuItem *m_parentItem;
		/**
		 * 是否弹出
		 */
		bool m_popup;
		/**
		* 自动适应位置和大小
		* @param  menu  菜单
		*/
		virtual void adjust(FCMenu *menu);
		/**
		* 检查图层是否具有焦点
		* @param  items  控件集合
		*/
		virtual bool checkDivFocused(ArrayList<FCMenuItem*> items);
		/**
		* 检查焦点
		* @param  control  控件
		*/
		virtual bool checkFocused(FCView *control);
		/**
		* 关闭网格控件
		* @param  items  菜单集合
		*/
		virtual bool closeMenus(ArrayList<FCMenuItem*> items);
	protected:
		/**
		* 调用菜单的触摸事件
		* @param  eventID   事件ID
		* @param  item   菜单项
         * @param  touchInfo   触摸信息
        */
		void callMenuItemTouchEvent(int eventID, FCMenuItem *item, FCTouchInfo touchInfo);
	public:
	    /**
		 * 菜单项
		 */
		ArrayList<FCMenuItem*> m_items;
		/*
		* 构造函数
		*/
		FCMenu();
		/*
		* 析构函数
		*/
		virtual ~FCMenu();
		/**
		 * 获取是否自动隐藏
		 */
		virtual bool autoHide();
		/**
		 * 设置是否自动隐藏
		 */
		virtual void setAutoHide(bool autoHide);
		/**
		 * 获取父菜单项
		 */
		virtual FCMenuItem* getParentItem();
		/**
		 * 设置父菜单项
		 */
		virtual void setParentItem(FCMenuItem *parentItem);
		/**
		 * 获取是否弹出
		 */
		virtual bool isPopup();
		/**
		 * 设置是否弹出
		 */
		virtual void setPopup(bool popup);
	public:
		/**
		* 添加项
		* @param  item  菜单项
		*/
		virtual void addItem(FCMenuItem *item);
		/**
		 * 清除所有的项
		 */
		virtual void clearItems();
		/**
		 * 创建菜单
		 */
		virtual FCMenu* createDropDownMenu();
		/**
		 * 获取控件类型
		 */
		virtual String getControlType();
		/**
		 * 获取所有的菜单项
		 */
		virtual ArrayList<FCMenuItem*> getItems();
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
		* @param  item   菜单项
		*/
		virtual void insertItem(int index, FCMenuItem *item);
		/**
		 * 是否不处理自动隐藏
		 */
		virtual bool onAutoHide();
		/*
		* 加载方法
		*/
		virtual void onLoad();
		/**
		* 菜单点击方法
		* @param  item   菜单项
         * @param  touchInfo   触摸信息
        */
		virtual void onMenuItemClick(FCMenuItem *item, FCTouchInfo touchInfo);
		/**
		* 菜单触摸移动方法
		* @param  item   菜单项
         * @param  touchInfo   触摸信息
        */
		virtual void onMenuItemTouchMove(FCMenuItem *item, FCTouchInfo touchInfo);
		/**
        * 触摸点击方法
        * @param  touchInfo   触摸信息
        */
		virtual void onTouchDown(FCTouchInfo touchInfo);
		/**
		* 秒表方法
		* @param  timerID   秒表ID
        */
		virtual void onTimer(int timerID);
		/**
		 * 可见状态改变方法
		 */
		virtual void onVisibleChanged();
		/**
		* 移除菜单项
		* @param  item   菜单项
        */
		virtual void removeItem(FCMenuItem *item);
		/**
		* 设置属性值
		* @param  name   属性名称
		* @param  value   属性值
		*/
		virtual void setProperty(const String& name, const String& value);
	};
}

#endif