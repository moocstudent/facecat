/*基于捂脸猫FaceCat框架 v1.0
 捂脸猫创始人-矿洞程序员-脉脉KOL-陶德 (微信号:suade1984);
 */

using System;
using System.Collections.Generic;
using System.Text;
using FaceCat;
using System.Xml;

namespace FaceCat
{
    /// <summary>
    /// 图形模板类
    /// </summary>
    public class UITemplate
    {
        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="xml">XML对象</param>
        /// <param name="control">控件</param>
        public static void AttachTemplate(UIXmlEx xml, FCView control)
        {
            XmlNode node = xml.Nodes[control];
            String newType = control.getControlType().ToLower();
            if (newType == "grid")
            {
                GridTemplate(control as FCGrid, xml, node);
            }
            else if (newType == "layoutdiv")
            {
                LayoutDivTemplate(control as FCLayoutDiv, xml, node);
            }
            else if (newType == "splitlayoutdiv")
            {
                SplitLayoutDivTemplate(control as FCSplitLayoutDiv, xml, node);
            }
            else if (newType == "tablelayoutdiv")
            {
                TableLayoutDivTemplate(control as FCTableLayoutDiv, xml, node);
            }
            else if (newType == "tabcontrol")
            {
                TabControlTemplate(control as FCTabControl, xml, node);
            }
            else if (newType == "tree")
            {
                TreeTemplate(control as FCTree, xml, node);
            }
            else if (newType == "div")
            {
                control.Size = new FCSize(100, 100);
            }
            else if (newType == "calendar")
            {
                control.Size = new FCSize(200, 200);
            }
            else if (newType == "groupbox")
            {
                control.Size = new FCSize(100, 100);
            }
            else
            {
                control.Size = new FCSize(100, 20);
            }
        }

        /// <summary>
        /// 创建控件名称
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="xml">XML对象</param>
        /// <returns>控件名称</returns>
        public static String CreateControlName(FCView control, UIXmlEx xml)
        {
            int count = 0;
            String controlType = control.getControlType();
            String controlName = controlType;
            while (xml.findControl(controlName) != null)
            {
                if (count > 0)
                {
                    controlName = controlType + (count + 1).ToString();
                }
                count++;
            }
            return controlName;
        }

        /// <summary>
        /// 加载表格的模板
        /// </summary>
        /// <param name="node">节点</param>
        public static void GridTemplate(FCGrid grid, UIXmlEx xml, XmlNode node)
        {
            XmlDocument xmlDoc = node.OwnerDocument;
            grid.Size = new FCSize(200, 200);
            FCGridColumn column = new FCGridColumn("Column1");
            grid.addColumn(column);
            FCGridColumn column2 = new FCGridColumn("Column2");
            grid.addColumn(column2);
            grid.update();
            FCGridRow row = new FCGridRow();
            grid.addRow(row);
            FCGridStringCell cell = new FCGridStringCell();
            cell.setString("Cell1");
            row.addCell(0, cell);
            FCGridStringCell cell2 = new FCGridStringCell();
            cell2.setString("Cell2");
            row.addCell(1, cell2);
            grid.update();
            XmlNode columnsNode = xmlDoc.CreateNode(XmlNodeType.Element, "tr", "");
            node.AppendChild(columnsNode);
            XmlNode column1Node = xmlDoc.CreateNode(XmlNodeType.Element, "th", "");
            column1Node.InnerText = "Column1";
            columnsNode.AppendChild(column1Node);
            XmlNode column2Node = xmlDoc.CreateNode(XmlNodeType.Element, "th", "");
            column2Node.InnerText = "Column2";
            columnsNode.AppendChild(column2Node);
            XmlNode rowNode = xmlDoc.CreateNode(XmlNodeType.Element, "tr", "");
            node.AppendChild(rowNode);
            XmlNode cellNode1 = xmlDoc.CreateNode(XmlNodeType.Element, "td", "");
            rowNode.AppendChild(cellNode1);
            cellNode1.InnerText = "Cell1";
            XmlNode cellNode2 = xmlDoc.CreateNode(XmlNodeType.Element, "td", "");
            rowNode.AppendChild(cellNode2);
            cellNode2.InnerText = "Cell2";
        }

        /// <summary>
        /// 加载布局控件的模板
        /// </summary>
        /// <param name="layoutDiv">布局控件</param>
        /// <param name="node">节点</param>
        public static void LayoutDivTemplate(FCLayoutDiv layoutDiv, UIXmlEx xml, XmlNode node)
        {
            XmlDocument xmlDoc = node.OwnerDocument;
            layoutDiv.Size = new FCSize(200, 200);
            FCDiv div1 = new FCDiv();
            div1.Name = CreateControlName(div1, xml);
            div1.Size = new FCSize(100, 100);
            layoutDiv.addControl(div1);
            XmlNode div1Node = xmlDoc.CreateNode(XmlNodeType.Element, "div", "");
            node.AppendChild(div1Node);
            XmlAttribute xmlAtr4 = xmlDoc.CreateAttribute("name");
            xmlAtr4.Value = div1.Name;
            div1Node.Attributes.Append(xmlAtr4);
            XmlAttribute xmlAtr5 = xmlDoc.CreateAttribute("size");
            xmlAtr5.Value = "100,100";
            div1Node.Attributes.Append(xmlAtr5);
            xml.m_controls.Add(div1);
            xml.Nodes[div1] = div1Node;
            FCDiv div2 = new FCDiv();
            div2.Name = CreateControlName(div2, xml);
            div2.Size = new FCSize(100, 100);
            layoutDiv.addControl(div2);
            XmlNode div2Node = xmlDoc.CreateNode(XmlNodeType.Element, "div", "");
            node.AppendChild(div2Node);
            XmlAttribute xmlAtr6 = xmlDoc.CreateAttribute("name");
            xmlAtr6.Value = div2.Name;
            div2Node.Attributes.Append(xmlAtr6);
            XmlAttribute xmlAtr7 = xmlDoc.CreateAttribute("size");
            xmlAtr7.Value = "100,100";
            div2Node.Attributes.Append(xmlAtr7);
            xml.m_controls.Add(div2);
            xml.Nodes[div2] = div2Node;
            layoutDiv.update();
        }

        /// <summary>
        /// 加载分割布局控件的模板
        /// </summary>
        /// <param name="splitLayoutDiv">分割布局控件</param>
        /// <param name="node">节点</param>
        public static void SplitLayoutDivTemplate(FCSplitLayoutDiv splitLayoutDiv, UIXmlEx xml, XmlNode node)
        {
            XmlDocument xmlDoc = node.OwnerDocument;
            splitLayoutDiv.Size = new FCSize(200, 200);
            splitLayoutDiv.LayoutStyle = FCLayoutStyle.TopToBottom;
            splitLayoutDiv.Splitter.Location = new FCPoint(0, 100);
            splitLayoutDiv.Splitter.Size = new FCSize(200, 1);
            XmlAttribute xmlAtr1 = xmlDoc.CreateAttribute("layoutstyle");
            xmlAtr1.Value = "toptobottom";
            node.Attributes.Append(xmlAtr1);
            XmlAttribute xmlAtr2 = xmlDoc.CreateAttribute("size");
            xmlAtr2.Value = "200,200";
            node.Attributes.Append(xmlAtr2);
            XmlAttribute xmlAtr3 = xmlDoc.CreateAttribute("splitterposition");
            xmlAtr3.Value = "100,1";
            node.Attributes.Append(xmlAtr3);
            FCDiv div1 = new FCDiv();
            div1.Name = CreateControlName(div1, xml);
            div1.Size = new FCSize(200, 100);
            splitLayoutDiv.addControl(div1);
            XmlNode div1Node = xmlDoc.CreateNode(XmlNodeType.Element, "div", "");
            node.AppendChild(div1Node);
            XmlAttribute xmlAtr4 = xmlDoc.CreateAttribute("name");
            xmlAtr4.Value = div1.Name;
            div1Node.Attributes.Append(xmlAtr4);
            XmlAttribute xmlAtr5 = xmlDoc.CreateAttribute("size");
            xmlAtr5.Value = "200,100";
            div1Node.Attributes.Append(xmlAtr5);
            xml.m_controls.Add(div1);
            xml.Nodes[div1] = div1Node;
            FCDiv div2 = new FCDiv();
            div2.Name = CreateControlName(div2, xml);
            div2.Size = new FCSize(200, 100);
            splitLayoutDiv.addControl(div2);
            XmlNode div2Node = xmlDoc.CreateNode(XmlNodeType.Element, "div", "");
            node.AppendChild(div2Node);
            XmlAttribute xmlAtr6 = xmlDoc.CreateAttribute("name");
            xmlAtr6.Value = div2.Name;
            div2Node.Attributes.Append(xmlAtr6);
            XmlAttribute xmlAtr7 = xmlDoc.CreateAttribute("size");
            xmlAtr7.Value = "200,100";
            div2Node.Attributes.Append(xmlAtr7);
            xml.m_controls.Add(div2);
            xml.Nodes[div2] = div2Node;
            splitLayoutDiv.FirstControl = div1;
            splitLayoutDiv.SecondControl = div2;
            splitLayoutDiv.update();
        }

        /// <summary>
        /// 多页夹控件的模板
        /// </summary>
        /// <param name="tabControl">多页夹控件</param>
        /// <param name="node">节点</param>
        public static void TabControlTemplate(FCTabControl tabControl, UIXmlEx xml, XmlNode node)
        {
            XmlDocument xmlDoc = node.OwnerDocument;
            tabControl.Size = new FCSize(200, 200);
            FCTabPage tabPage = new FCTabPage();
            tabControl.addControl(tabPage);
            tabPage.Name = CreateControlName(tabPage, xml);
            tabPage.Text = tabPage.Name;
            XmlNode tabPageNode = xmlDoc.CreateNode(XmlNodeType.Element, "div", "");
            node.AppendChild(tabPageNode);
            XmlAttribute xmlAtr1 = xmlDoc.CreateAttribute("name");
            xmlAtr1.Value = tabPage.Name;
            tabPageNode.Attributes.Append(xmlAtr1);
            XmlAttribute xmlAtr2 = xmlDoc.CreateAttribute("text");
            xmlAtr2.Value = tabPage.Name;
            tabPageNode.Attributes.Append(xmlAtr2);
            XmlAttribute xmlAtr3 = xmlDoc.CreateAttribute("type");
            xmlAtr3.Value = "tabpage";
            tabPageNode.Attributes.Append(xmlAtr3);
            xml.m_controls.Add(tabPage);
            xml.Nodes[tabPage] = tabPageNode;
            tabControl.update();
        }

        /// <summary>
        /// 表格布局控件的模板
        /// </summary>
        /// <param name="tableLayoutDiv">表格布局控件</param>
        /// <param name="xml">XML</param>
        /// <param name="node">节点</param>
        public static void TableLayoutDivTemplate(FCTableLayoutDiv tableLayoutDiv, UIXmlEx xml, XmlNode node)
        {
            XmlDocument xmlDoc = node.OwnerDocument;
            tableLayoutDiv.Size = new FCSize(200, 200);
            tableLayoutDiv.ColumnsCount = 2;
            tableLayoutDiv.RowsCount = 2;
            tableLayoutDiv.ColumnStyles.Add(new FCColumnStyle(FCSizeType.PercentSize, 0.5f));
            tableLayoutDiv.ColumnStyles.Add(new FCColumnStyle(FCSizeType.PercentSize, 0.5f));
            tableLayoutDiv.RowStyles.Add(new FCRowStyle(FCSizeType.PercentSize, 0.5f));
            tableLayoutDiv.RowStyles.Add(new FCRowStyle(FCSizeType.PercentSize, 0.5f));
            XmlAttribute xmlAtr1 = xmlDoc.CreateAttribute("columnscount");
            xmlAtr1.Value = "2";
            node.Attributes.Append(xmlAtr1);
            XmlAttribute xmlAtr2 = xmlDoc.CreateAttribute("rowscount");
            xmlAtr2.Value = "2";
            node.Attributes.Append(xmlAtr2);
            XmlNode columnsStylesNode = xmlDoc.CreateNode(XmlNodeType.Element, "columnstyles", "");
            node.AppendChild(columnsStylesNode);
            XmlNode columnStyleNode1 = xmlDoc.CreateNode(XmlNodeType.Element, "columnstyle", "");
            columnsStylesNode.AppendChild(columnStyleNode1);
            XmlAttribute columnStyleNodeAtr1 = xmlDoc.CreateAttribute("sizetype");
            columnStyleNodeAtr1.Value = "percentsize";
            columnStyleNode1.Attributes.Append(columnStyleNodeAtr1);
            XmlAttribute columnStyleNodeAtr2 = xmlDoc.CreateAttribute("width");
            columnStyleNodeAtr2.Value = "0.5";
            columnStyleNode1.Attributes.Append(columnStyleNodeAtr2);
            XmlNode columnStyleNode2 = xmlDoc.CreateNode(XmlNodeType.Element, "columnstyle", "");
            columnsStylesNode.AppendChild(columnStyleNode2);
            XmlAttribute columnStyleNodeAtr3 = xmlDoc.CreateAttribute("sizetype");
            columnStyleNodeAtr3.Value = "percentsize";
            columnStyleNode2.Attributes.Append(columnStyleNodeAtr3);
            XmlAttribute columnStyleNodeAtr4 = xmlDoc.CreateAttribute("width");
            columnStyleNodeAtr4.Value = "0.5";
            columnStyleNode2.Attributes.Append(columnStyleNodeAtr4);
            XmlNode rowsStylesNode = xmlDoc.CreateNode(XmlNodeType.Element, "rowstyles", "");
            node.AppendChild(rowsStylesNode);
            XmlNode rowStyleNode1 = xmlDoc.CreateNode(XmlNodeType.Element, "rowstyle", "");
            rowsStylesNode.AppendChild(rowStyleNode1);
            XmlAttribute rowStyleNodeAtr1 = xmlDoc.CreateAttribute("sizetype");
            rowStyleNodeAtr1.Value = "percentsize";
            rowStyleNode1.Attributes.Append(rowStyleNodeAtr1);
            XmlAttribute rowStyleNodeAtr2 = xmlDoc.CreateAttribute("height");
            rowStyleNodeAtr2.Value = "0.5";
            rowStyleNode1.Attributes.Append(rowStyleNodeAtr2);
            XmlNode rowStyleNode2 = xmlDoc.CreateNode(XmlNodeType.Element, "rowstyle", "");
            rowsStylesNode.AppendChild(rowStyleNode2);
            XmlAttribute rowStyleNodeAtr3 = xmlDoc.CreateAttribute("sizetype");
            rowStyleNodeAtr3.Value = "percentsize";
            rowStyleNode2.Attributes.Append(rowStyleNodeAtr3);
            XmlAttribute rowStyleNodeAtr4 = xmlDoc.CreateAttribute("height");
            rowStyleNodeAtr4.Value = "0.5";
            rowStyleNode2.Attributes.Append(rowStyleNodeAtr4);
            XmlNode childsNodes = xmlDoc.CreateNode(XmlNodeType.Element, "childs", "");
            node.AppendChild(childsNodes);

            FCDiv div1 = new FCDiv();
            div1.Name = CreateControlName(div1, xml);
            div1.Size = new FCSize(50, 50);
            tableLayoutDiv.addControl(div1);
            XmlNode div1Node = xmlDoc.CreateNode(XmlNodeType.Element, "div", "");
            childsNodes.AppendChild(div1Node);
            XmlAttribute divAtr1 = xmlDoc.CreateAttribute("name");
            divAtr1.Value = div1.Name;
            div1Node.Attributes.Append(divAtr1);
            xml.m_controls.Add(div1);
            xml.Nodes[div1] = div1Node;
            FCDiv div2 = new FCDiv();
            div2.Name = CreateControlName(div2, xml);
            div2.Size = new FCSize(50, 50);
            tableLayoutDiv.addControl(div2);
            XmlNode div2Node = xmlDoc.CreateNode(XmlNodeType.Element, "div", "");
            childsNodes.AppendChild(div2Node);
            XmlAttribute divAtr2 = xmlDoc.CreateAttribute("name");
            divAtr2.Value = div2.Name;
            div2Node.Attributes.Append(divAtr2);
            xml.m_controls.Add(div2);
            xml.Nodes[div2] = div2Node;
            FCDiv div3 = new FCDiv();
            div3.Name = CreateControlName(div3, xml);
            div3.Size = new FCSize(50, 50);
            tableLayoutDiv.addControl(div3);
            XmlNode div3Node = xmlDoc.CreateNode(XmlNodeType.Element, "div", "");
            childsNodes.AppendChild(div3Node);
            XmlAttribute divAtr3 = xmlDoc.CreateAttribute("name");
            divAtr3.Value = div3.Name;
            div3Node.Attributes.Append(divAtr3);
            xml.m_controls.Add(div3);
            xml.Nodes[div3] = div2Node;
            FCDiv div4 = new FCDiv();
            div4.Name = CreateControlName(div4, xml);
            div4.Size = new FCSize(50, 59);
            tableLayoutDiv.addControl(div4);
            XmlNode div4Node = xmlDoc.CreateNode(XmlNodeType.Element, "div", "");
            childsNodes.AppendChild(div4Node);
            XmlAttribute divAtr4 = xmlDoc.CreateAttribute("name");
            divAtr4.Value = div4.Name;
            div4Node.Attributes.Append(divAtr4);
            xml.m_controls.Add(div4);
            xml.Nodes[div4] = div4Node;
            tableLayoutDiv.update();
        }

        /// <summary>
        /// 加载树的模板
        /// </summary>
        /// <param name="tree">树</param>
        /// <param name="node">节点</param>
        public static void TreeTemplate(FCTree tree, UIXmlEx xml, XmlNode node)
        {
            XmlDocument xmlDoc = node.OwnerDocument;
            tree.Size = new FCSize(200, 200);
            FCGridColumn column = new FCGridColumn("Column1");
            column.AllowSort = false;
            tree.addColumn(column);
            FCGridColumn column2 = new FCGridColumn("Column2");
            column2.AllowSort = false;
            tree.addColumn(column2);
            tree.update();
            FCTreeNode treeNode = new FCTreeNode();
            treeNode.Text = "Node1";
            tree.appendNode(treeNode);
            FCTreeNode subTreeNode = new FCTreeNode();
            subTreeNode.Text = "Node2";
            treeNode.appendNode(subTreeNode);
            tree.update();
            XmlNode columnsNode = xmlDoc.CreateNode(XmlNodeType.Element, "tr", "");
            node.AppendChild(columnsNode);
            XmlNode column1Node = xmlDoc.CreateNode(XmlNodeType.Element, "th", "");
            columnsNode.AppendChild(column1Node);
            XmlAttribute xmlAtr1 = xmlDoc.CreateAttribute("text");
            xmlAtr1.Value = "Column1";
            column1Node.Attributes.Append(xmlAtr1);
            XmlNode column2Node = xmlDoc.CreateNode(XmlNodeType.Element, "th", "");
            columnsNode.AppendChild(column2Node);
            XmlAttribute xmlAtr2 = xmlDoc.CreateAttribute("text");
            xmlAtr2.Value = "Column2";
            column2Node.Attributes.Append(xmlAtr2);
            XmlNode nodesNode = xmlDoc.CreateNode(XmlNodeType.Element, "nodes", "");
            node.AppendChild(nodesNode);
            XmlNode nodeNode = xmlDoc.CreateNode(XmlNodeType.Element, "node", "");
            nodesNode.AppendChild(nodeNode);
            XmlAttribute xmlAtr3 = xmlDoc.CreateAttribute("text");
            xmlAtr3.Value = "Node1";
            nodeNode.Attributes.Append(xmlAtr3);
            XmlNode subNodeNode = xmlDoc.CreateNode(XmlNodeType.Element, "node", "");
            nodeNode.AppendChild(subNodeNode);
            XmlAttribute xmlAtr4 = xmlDoc.CreateAttribute("text");
            xmlAtr4.Value = "Node2";
            subNodeNode.Attributes.Append(xmlAtr4);
        }
    }
}
