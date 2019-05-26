/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCTABCONTROL_H__
#define __FCTABCONTROL_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\core\\FCStr.h"
#include "..\\div\\FCDiv.h"
#include "FCTabPage.h"

namespace FaceCat{
    /**
	* 页的布局位置
	*/
	enum TabPageLayout{
	    /**
		 * 下方
		 */
		TabPageLayout_Bottom,
		/**
		 * 左侧
		 */
		TabPageLayout_Left,
		/**
		 * 右侧
		 */
		TabPageLayout_Right,
		/**
		 * 顶部
		 */
		TabPageLayout_Top
	};
	
	class FCTabPage;

	/*
	* 多页夹控件
	*/
	class FCTabControl:public FCDiv{
	private:
	    /**
		 * 秒表ID
		 */
		int m_timerID;
	protected:
	    /**
		 * 动画状态
		 */
		int m_animationState;
		/**
		 * 页的布局位置
		 */
		TabPageLayout m_layout;
		/**
		 * 选中的索引
		 */
		int m_selectedIndex;
		/**
		 * 是否使用动画
		 */
		bool m_useAnimation;
		/**
		 * 绘制移动
		 */
		void drawMoving();
		/*
		* 获取索引
		*/
		int getTabPageIndex(FCTabPage *tabPage);
	public:
	    /**
		 * 所有页
		 */
		ArrayList<FCTabPage*> m_tabPages;
		/* 
		* 创建多页夹
		*/
		FCTabControl();
		/*
		* 删除多页夹
		*/
		virtual ~FCTabControl();
		/**
		 * 获取页的布局位置
		 */
		virtual TabPageLayout getLayout();
		/**
		 * 设置页的布局位置
		 */
		virtual void setLayout(TabPageLayout layout);
		/**
		 * 获取选中的索引
		 */
		virtual int getSelectedIndex();
		/**
		 * 设置选中的索引
		 */
		virtual void setSelectedIndex(int selectedIndex);
		/**
		 * 获取选中的页
		 */
		virtual FCTabPage* getSelectedTabPage();
		/**
		 * 设置选中的页
		 */
		virtual void setSelectedTabPage(FCTabPage *selectedTabPage);
		/**
		 * 获取是否使用动画
		 */
		virtual bool useAnimation();
		/**
		 * 设置是否使用动画
		 */
		virtual void setUseAnimation(bool useAnimation);
	public:
	    /**
		 * 添加控件方法
		 */
		virtual void addControl(FCView *control);
		/**
		 * 获取控件类型
		 */
		virtual String getControlType();
		/**
		 * 清除控件
		 */
		virtual void clearControls();
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
		* 插入控件
		* @param  index  索引
		* @param  control  控件
		*/
		virtual void insertControl(int index, FCView *control);
		/**
		 * 开始拖动页头
		 */
        virtual void onDragTabHeaderBegin(FCTabPage *tabPage);
        /**
		 * 结束拖动页头
		 */
        virtual void onDragTabHeaderEnd(FCTabPage *tabPage);
        /**
		 * 页头拖动中
		 */
        virtual void onDraggingTabHeader(FCTabPage *tabPage);
        /**
		 * 添加控件方法
		 */
		virtual void onLoad();
		/**
		 * 页改变方法
		 */
		virtual void onSelectedTabPageChanged();
		/**
		 * 秒表方法
		 */
		virtual void onTimer(int timerID);
		/**
		 * 移除控件
		 */
		virtual void removeControl(FCView *control);
		/**
		* 设置属性
		* @param  name  属性名称
		* @param  value  属性值
		*/
		virtual void setProperty(const String& name, const String& value);
		/**
		 * 重新布局
		 */
		virtual void update();
	};
}

#endif