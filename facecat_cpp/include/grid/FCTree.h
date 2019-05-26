/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCTREE_H__
#define __FCTREE_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\grid\\FCGrid.h"
#include "..\\grid\\FCGridColumn.h"
#include "..\\grid\\FCGridRow.h"
#include "..\\grid\\FCGridCell.h"
#include "..\\grid\\FCGridCellExtends.h"
#include "FCTreeNode.h"

namespace FaceCat{
	class FCTreeNode;

	/*
	* 树控件
	*/
	class FCTree:public FCGrid{
	protected:
	    /**
		 * 是否在节点旁显示复选框
		 */
		bool m_checkBoxes;
		/**
		 * 复选框的大小
		 */
		FCSize m_checkBoxSize;
		/**
		 * 选框选中时的图片
		 */
		String m_checkedImage;
		/**
		 * 折叠节点的图片
		 */
		String m_collapsedNodeImage;
		/**
		 * 展开节点的图片
		 */
		String m_expendedNodeImage;
		/**
		 * 正在移动的节点
		 */
		FCTreeNode *m_movingNode;
		/**
		 * 节点的大小
		 */
		FCSize m_nodeSize;
		/**
		 * 复选框未选中时的图片
		 */
		String m_unCheckedImage;
	public:
		/*
		* 节点
		*/
		ArrayList<FCTreeNode*> m_nodes;
		/*
		* 构造函数
		*/
		FCTree();
		/*
		* 析构函数
		*/
		virtual ~FCTree();
		/**
		 * 获取是否在节点旁显示复选框
		 */
		virtual bool hasCheckBoxes();
		/**
		 * 设置是否在节点旁显示复选框
		 */
		virtual void setCheckBoxes(bool checkBoxes);
		/**
		 * 获取复选框的大小
		 */
		virtual FCSize getCheckBoxSize();
		/**
		 * 设置复选框的大小
		 */
		virtual void setCheckBoxSize(FCSize checkBoxSize);
		/**
		 * 获取复选框选中时的图片
		 */
		virtual String getCheckedImage();
		/**
		 * 设置复选框选中时的图片
		 */
		virtual void setCheckedImage(const String& checkedImage);
		/**
		 * 获取折叠节点的图片
		 */
		virtual String getCollapsedNodeImage();
		/**
		 * 设置折叠节点的图片
		 */
		virtual void setCollapsedNodeImage(const String& collapsedNodeImage);
		/**
		 * 获取展开节点的图片
		 */
		virtual String getExpendedNodeImage();
		/**
		 * 设置展开节点的图片
		 */
		virtual void setExpendedNodeImage(const String& expendedNodeImage);
		/**
		 * 获取节点的大小
		 */
		virtual FCSize getNodeSize();
		/**
		 * 设置节点的大小
		 */
		virtual void setNodeSize(FCSize nodeSize);
		/**
		 * 获取选中的节点
		 */
		virtual ArrayList<FCTreeNode*> getSelectedNodes();
		/**
		 * 设置选中的节点
		 */
		virtual void setSelectedNodes(ArrayList<FCTreeNode*> selectedNodes);
		/**
		 * 获取复选框未选中时的图片
		 */
		virtual String getUnCheckedImage();
		/**
		 * 设置复选框未选中时的图片
		 */
		virtual void setUnCheckedImage(const String& unCheckedImage);
	public:
	    /**
		 * 添加节点
		 */
		void appendNode(FCTreeNode *node);
		/**
		 * 清除所有节点
		 */
		void clearNodes();
		/**
		 * 折叠节点
		 */
		void collapse();
		/**
		 * 折叠所有节点
		 */
		void collapseAll();
		/**
		 * 展开节点
		 */
		void expend();
		/**
		 * 展开所有节点
		 */
		void expendAll();
		/**
		 * 获取子节点
		 */
		ArrayList<FCTreeNode*> getChildNodes();
		/**
		 * 获取控件类型
		 */
		virtual String getControlType();
		/**
		 * 获取节点的索引
		 */
		int getNodeIndex(FCTreeNode *node);
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
		 * 插入节点
		 */
		void insertNode(int index, FCTreeNode *node);
		/**
		 * 单元格触摸按下方法
		 */
		virtual void onCellTouchDown(FCGridCell *cell, FCTouchInfo touchInfo);
		/**
		 * 单元格触摸移动方法
		 */
		virtual void onCellTouchMove(FCGridCell *cell, FCTouchInfo touchInfo);
		/**
		 * 单元格触摸抬起方法
		 */
		virtual void onCellTouchUp(FCGridCell *cell, FCTouchInfo touchInfo);
		/**
		 * 重绘前景方法
		 */
		virtual void onPaintForeground(FCPaint *paint, const FCRect& clipRect);
		/**
		 * 绘制编辑文本框
		 */
		virtual void onPaintEditTextBox(FCGridCell *cell, FCPaint *paint, const FCRect& rect, const FCRect& clipRect);
		/**
		 * 移除节点
		 */
		void removeNode(FCTreeNode *node);
		/**
		 * 设置属性
		 */
		virtual void setProperty(const String& name, const String& value);
	};
}

#endif