/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCTREENODE_H__
#define __FCTREENODE_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "..\\grid\\FCGrid.h"
#include "..\\grid\\FCGridRow.h"
#include "..\\grid\\FCGridCell.h"
#include "..\\btn\\FCCheckBox.h"
#include "FCTree.h"

namespace FaceCat{
	class FCTree;

	/*
	* 树节点
	*/
	class FCTreeNode : public FCGridControlCell{
	protected:
	    /**
		 * 是否可以拖入节点
		 */
		bool m_allowDragIn;
		/**
		 * 是否可以拖出节点
		 */
		bool m_allowDragOut;
		/**
		 * 复选框是否选中
		 */
		bool m_checked;
		/**
		 * 子节点
		 */
		ArrayList<FCTreeNode*> m_nodes;
		/**
		 * 是否展开节点
		 */
		bool m_expended;
		/**
		 * 文字缩进距离
		 */
		int m_indent;
		/**
		 * 父节点
		 */
		FCTreeNode *m_parent;
		/**
		 * 目标列
		 */
		FCGridColumn *m_targetColumn;
		/**
		 * 文字
		 */
		String m_text;
		/**
		 * 树控件
		 */
		FCTree *m_tree;
		/**
		 * 值
		 */
		String m_value;
		/**
		 * 选中所有子节点
		 */
		void checkChildNodes(ArrayList<FCTreeNode*> nodes, bool isChecked);
		/**
		 * 折叠子节点
		 */
		void collapseChildNodes(ArrayList<FCTreeNode*> nodes, bool collapseAll);
		/**
		 * 展开所有的节点
		 */
		void expendChildNodes(ArrayList<FCTreeNode*> nodes, bool parentExpened, bool expendAll);
		/**
		 * 获取最后节点的索引
		 */
		FCTreeNode* getLastNode(ArrayList<FCTreeNode*> nodes);
	public:
		/*
		* 构造函数
		*/
		FCTreeNode();
		/*
		* 析构函数
		*/
		virtual ~FCTreeNode();
		/**
		 * 获取是否可以拖入节点
		 */
		virtual bool allowDragIn();
		/**
		 * 设置是否可以拖入节点
		 */
		virtual void setAllowDragIn(bool allowDragIn);
		/**
		 * 获取是否可以拖出节点
		 */
		virtual bool allowDragOut();
		/**
		 * 设置是否可以拖出节点
		 */
		virtual void setAllowDragOut(bool allowDragOut);
		/**
		 * 获取复选框是否选中
		 */
		virtual bool isChecked();
		/**
		 * 设置复选框是否选中
		 */
		virtual void setChecked(bool checked);
		/**
		 * 获取是否展开节点
		 */
		virtual bool isExpended();
		/**
		 * 设置是否展开节点
		 */
		virtual void setExpended(bool expended);
		/**
		 * 获取文字缩进距离
		 */
		virtual int getIndent();
		/**
		 * 获取父节点
		 */
		virtual FCTreeNode* getParent();
		/**
		 * 设置父节点
		 */
		virtual void setParent(FCTreeNode *parent);
		/**
		 * 获取目标列
		 */
		virtual FCGridColumn* getTargetColumn();
		/**
		 * 设置目标列
		 */
		virtual void setTargetColumn(FCGridColumn *targetColumn);
		/**
		 * 获取树控件
		 */
		virtual FCTree* getTree();
		/**
		 * 设置树控件
		 */
		virtual void setTree(FCTree *tree);
		/**
		 * 获取值
		 */
		virtual String getValue();
		/**
		 * 设置值
		 */
		virtual void setValue(const String& value);
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
		 * 获取节点的索引
		 */
		int getNodeIndex(FCTreeNode *node);
		/**
		 * 获取要绘制的文本
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
		 * 插入节点
		 */
		void insertNode(int index, FCTreeNode *node);
		/**
		 * 父节点是否可见
		 */
		bool isNodeVisible(FCTreeNode *node);
		/**
		 * 添加节点
		 */
		virtual void onAddingNode(int index);
		/**
		 * 绘制复选框
		 */
		virtual void onPaintCheckBox(FCPaint *paint, const FCRect& rect);
		/**
		 * 绘制节点
		 */
		virtual void onPaintNode(FCPaint *paint, const FCRect& rect);
		/**
		 * 重绘方法
		 */
		virtual void onPaint(FCPaint *paint, const FCRect& rect, const FCRect& clipRect, bool isAlternate);
		/**
		 * 移除节点方法
		 */
		virtual void onRemovingNode();
		/**
		 * 移除节点
		 */
		void removeNode(FCTreeNode *node);
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
}

#endif