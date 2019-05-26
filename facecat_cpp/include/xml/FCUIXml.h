/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __UIXML_H__
#define __UIXML_H__
#pragma once
#include "..\\..\\stdafx.h"
#include "FCUIEvent.h"
#include "FCUIScript.h"
#include <atlstr.h>  
#import <msxml3.dll>
#include "..\\core\\FCLib.h"

namespace FaceCat{
	#define HRCALL(a, errmsg) \
	do { \
		hr = (a); \
		if (FAILED(hr)) { \
			dprintf( "%s:%d  HRCALL Failed: %s\n  0x%.8x = %s\n", \
					__FILE__, __LINE__, errmsg, hr, #a ); \
			goto clean; \
		} \
	} while (0)

	class FCUIEvent;
	class FCUIScript;

	/*
	* XML处理
	*/
	class FCUIXml{
	protected:
	    /**
		 * 控件列表
		 */
		ArrayList<FCView*> m_controls;
		/**
		 * 事件
		 */
		FCUIEvent *m_event;
		/**
		 * 方法库
		 */
		FCNative *m_native;
		/**
		 * 脚本
		 */
		FCUIScript *m_script;
		/**
		 * CSS样式
		 */
		HashMap<String, String> m_styles;
		/*
		* 创建FCGridBand
		*/
		void createBandedGridBands(IXMLDOMNode *node, FCView *control);
		/*
		 * 创建表格列
		 */
		void createBandedGridColumns(IXMLDOMNode *node, FCView *control);
		/*
		 * 创建图形的子属性 
		 */
		void createChartSubProperty(IXMLDOMNode *node, FCChart *chart);
		/**
		 * 创建菜单项
		 */
		void createMenuItem(IXMLDOMNode *node, FCMenu *menu, FCMenuItem *parentItem);
		/*
		 * 创建表格列
		 */
		void createGridColumns(IXMLDOMNode *node, FCView *control);
		/*
		 * 创建表格行
		 */
		void createGridRow(IXMLDOMNode *node, FCView *control);
		/*
		 * 创建表格行
		 */
		void createGridRows(IXMLDOMNode *node, FCView *control);
		/**
		 * 创建分割布局控件
		 */
		void createSplitLayoutSubProperty(IXMLDOMNode *node, FCSplitLayoutDiv *splitLayoutDiv);
		/**
		 * 创建控件
		 */
		void createTableLayoutSubProperty(IXMLDOMNode *node, FCTableLayoutDiv *tableLayoutDiv);
		/**
		 * 创建多页夹的页
		 */
		void createTabPage(IXMLDOMNode *node, FCView *control);
		/**
		 * 创建树的节点
		 */
		void createTreeNode(IXMLDOMNode *node, FCView *control, FCTreeNode *treeNode);
		/**
		 * 创建树的节点
		 */
		void createTreeNodes(IXMLDOMNode *node, FCView *control);
		/**
		 * 创建用户控件
		 */
		FCView* createUserControl(IXMLDOMNode *node);
		static void dprintf(char *format, ...);
		static IXMLDOMDocument* domFromCOM();	
	public:
		/*
		* 构造函数
		*/
		FCUIXml();
		/*
		* 析构函数
		*/
		virtual ~FCUIXml();
		/**
		 * 获取事件
		 */
		FCUIEvent* getEvent();
		/**
		 * 设置事件
		 */
		void setEvent(FCUIEvent *uiEvent);
		/**
		 * 获取方法库
		 */
		FCNative* getNative();
		/**
		 * 设置方法库
		 */
		void setNative(FCNative *native);
		/**
		 * 获取脚本
		 */
		FCUIScript* getScript();
		/**
		 * 设置脚本
		 */
		void setScript(FCUIScript *script);
		/*
		 * 获取XML的路径
		 */
		String getXmlPath();
	public:
		/**
		* 获取按钮
		* @params name   控件名称
		*/
		FCButton* getButton(const String& name);
		/**
		 * 获取图形控件
		 */
		FCChart* getChart(const String& name);
		/**
		 * 获取复选框
		 */
		FCCheckBox* getCheckBox(const String& name);
		/**
		 * 获取下拉控件
		 */
		FCComboBox* getComboBox(const String& name);
		/*
		 * 获取日期时间控件
		 */
		FCDateTimePicker* getDateTimePicker(const String& name);
		/**
		 * 获取图层
		 */
		FCDiv* getDiv(const String& name);
		/**
		 * 获取表格
		 */
		FCGrid* getGrid(const String& name);
		/**
		 * 获取组控件
		 */
		FCGroupBox* getGroupBox(const String& name);
		/**
		 * 获取标签
		 */
		FCLabel* getLabel(const String& name);
		/**
		 * 获取布局层
		 */
		FCLayoutDiv* getLayoutDiv(const String& name);
		/**
		 * 获取名称相似控件
		 */
		ArrayList<FCView*> getLikeControls(const String& name);
		/**
		 * 获取菜单控件
		 */
		FCMenu* getMenu(const String& name);
		/**
		 * 获取菜单项控件
		 */
		FCMenuItem* getMenuItem(const String& name);
		/**
		 * 获取单选按钮
		 */
		FCRadioButton* getRadioButton(const String& name);
		/**
		 * 获取数值控件
		 */
		FCSpin* getSpin(const String& name);
		/**
		 * 获取分割层
		 */
		FCSplitLayoutDiv* getSplitLayoutDiv(const String& name);
		/**
		 * 获取多页夹控件
		 */
		FCTabControl* getTabControl(const String& name);
		/**
		 * 获取表格层
		 */
		FCTableLayoutDiv* getTableLayoutDiv(const String& name);
		/**
		 * 获取夹控件
		 */
		FCTabPage* getTabPage(const String& name);
		/**
		 * 获取文本框
		 */
		FCTextBox* getTextBox(const String& name);
		/**
		 * 获取树控件
		 */
		FCTree* getTree(const String& name);
		/**
		 * 获取窗体
		 */
		FCWindow* getWindow(const String& name);
	public:
		/**
		* 是否包含控件
		* @params control  控件
		* @params node  节点
		*/
		virtual bool containsControl(FCView *control);
		/**
		 * 创建控件
		 */
		virtual FCView* createControl(IXMLDOMNode *node, const String& type);
		/**
		 * 创建控件框架
		 */
		void createNative();
		/**
		 * 创建子属性
		 */
		virtual void createSubProperty(IXMLDOMNode *node, FCView *control);
		/**
		 * 查找控件
		 */
		virtual FCView* findControl(const String& name);
		/**
		 * 获取属性
		 */
		HashMap<String, String> getAttributes(IXMLDOMNode *node);
		/**
		 * 获取所有的控件
		 */
		ArrayList<FCView*> getControls();
		/**
		 * 判断是否后设置属性
		 */
		virtual bool isAfterSetingAttribute(const String& name);
		/**
		 * 读取文件，加载到控件中
		 */
		virtual void loadFile(const String& fileName, FCView *control);
		/**
		 * 添加控件
		 */
		virtual void onAddControl(FCView *control, IXMLDOMNode *node);
		/**
		 * 读取文件体
		 */
		virtual void readBody(IXMLDOMNode *node, FCView *control);
		/**
		 * 读取子节点
		 */
		virtual void readChildNodes(IXMLDOMNode *node, FCView *control);
		/**
		 * 读取头部
		 */
		virtual void readHead(IXMLDOMNode *node, FCView *control);
		/**
		 * 读取XML
		 */
		virtual FCView* readNode(IXMLDOMNode *node, FCView* parent);
		/**
		 * 读取样式
		 */
		virtual void readStyle(IXMLDOMNode *node, FCView *control);
		/**
		 * 后设置属性
		 */
		virtual void setAttributesAfter(IXMLDOMNode *node, FCProperty *control);
		/**
		 * 前设置属性
		 */
		virtual void setAttributesBefore(IXMLDOMNode *node, FCProperty *control);
		/**
		 * 设置事件
		 */
		virtual void setEvents(IXMLDOMNode *node, FCProperty *control);
		/**
		 * 设置CSS样式
		 */
		virtual void setStyle(const String& style, FCProperty *control);
	};
}
#endif