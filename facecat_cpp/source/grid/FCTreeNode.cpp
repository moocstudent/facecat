#include "..\\..\\stdafx.h"
#include "..\\..\\include\\grid\\FCTreeNode.h"

namespace FaceCat{
	void FCTreeNode::checkChildNodes(ArrayList<FCTreeNode*> nodes, bool isChecked){
		int nodeSize = (int)nodes.size();
		for (int i = 0; i < nodeSize; i++){
			FCTreeNode *node = nodes.get(i);
			node->setChecked(isChecked);
			ArrayList<FCTreeNode*> childNodes = node->getChildNodes();
			if (childNodes.size() > 0){
				checkChildNodes(childNodes, isChecked);
			}
		}
	}

	void FCTreeNode::collapseChildNodes(ArrayList<FCTreeNode*> nodes, bool collapseAll){
		int nodeSize = (int)nodes.size();
		for (int i = 0; i < nodeSize; i++){
			FCTreeNode *node = nodes.get(i);
			if (collapseAll){
				node->setExpended(false);
			}
			node->getRow()->setVisible(false);
			ArrayList<FCTreeNode*> childNodes = node->getChildNodes();
			if (childNodes.size() > 0){
				collapseChildNodes(childNodes, collapseAll);
			}
		}
	}

	void FCTreeNode::expendChildNodes(ArrayList<FCTreeNode*> nodes, bool parentExpened, bool expendAll)
	{
		int nodeSize = (int)nodes.size();
		for (int i = 0; i < nodeSize; i++){
			FCTreeNode *node = nodes.get(i);
			bool pExpended = parentExpened;
			if (expendAll){
				pExpended = true;
				node->getRow()->setVisible(true);
				node->setExpended(true);
			}
			else{
				if (parentExpened){
					node->getRow()->setVisible(true);
				}
				else{
					node->getRow()->setVisible(false);
				}
				if (!node->isExpended()){
					pExpended = false;
				}
			}
			ArrayList<FCTreeNode*> childNodes = node->getChildNodes();
			if ((int)childNodes.size() > 0){
				expendChildNodes(childNodes, pExpended, expendAll);
			}
		}
	}

	FCTreeNode* FCTreeNode::getLastNode(ArrayList<FCTreeNode*> nodes){
		int size = (int)nodes.size();
		if (size > 0){
			for (int i = size - 1; i >= 0; i--){
                FCTreeNode *lastNode = nodes.get(i);
                if (lastNode->getRow()){
                    ArrayList<FCTreeNode*> childNodes = lastNode->getChildNodes();
                    FCTreeNode *subLastNode = getLastNode(childNodes);
                    if (subLastNode){
                        return subLastNode;
                    }
                    else{
                        return lastNode;
                    }
                }
            }
		}
		return 0;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCTreeNode::FCTreeNode(){
		m_allowDragIn = false;
		m_allowDragOut = false;
		m_checked = false;
		m_expended = true;
		m_indent = 0;
		m_parent = 0;
		m_targetColumn = 0;
		m_tree = 0;
	}

	FCTreeNode::~FCTreeNode(){
		m_nodes.clear();
		m_parent = 0;
		m_targetColumn = 0;
		m_tree = 0;
	}

	bool FCTreeNode::allowDragIn(){
		return m_allowDragIn;
	}

	void FCTreeNode::setAllowDragIn(bool allowDragIn){
		m_allowDragIn = allowDragIn;
	}

	bool FCTreeNode::allowDragOut(){
		return m_allowDragOut;
	}

	void FCTreeNode::setAllowDragOut(bool allowDragOut){
		m_allowDragOut = allowDragOut;
	}

	bool FCTreeNode::isChecked(){
		return m_checked;
	}

	void FCTreeNode::setChecked(bool checked){
		if(m_checked != checked){
			m_checked = checked;
			checkChildNodes(m_nodes, checked);
		}
	}

	bool FCTreeNode::isExpended(){
		return m_expended;
	}

	void FCTreeNode::setExpended(bool expended){
		m_expended = expended;
	}

	int FCTreeNode::getIndent(){
		return m_indent;
	}

	FCTreeNode* FCTreeNode::getParent(){
		return m_parent;
	}

	void FCTreeNode::setParent(FCTreeNode *parent){
		m_parent = parent;
	}

	FCGridColumn* FCTreeNode::getTargetColumn(){
		return m_targetColumn;
	}

	void FCTreeNode::setTargetColumn(FCGridColumn *targetColumn){
		m_targetColumn = targetColumn;
	}

	FCTree* FCTreeNode::getTree(){
		return m_tree;
	}

	void FCTreeNode::setTree(FCTree *tree){
		m_tree = tree;
	}

	String FCTreeNode::getValue(){
		return m_value;
	}

	void FCTreeNode::setValue(const String& value){
		m_value = value;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void FCTreeNode::appendNode(FCTreeNode *node){
		FCTreeNode *lastNode = getLastNode(m_nodes);
		node->setParent(this);
		node->setTree(m_tree);
		node->onAddingNode(-1);
		m_nodes.add(node);
	}

	void FCTreeNode::clearNodes(){
		while((int)m_nodes.size() > 0){
			removeNode(m_nodes.get((int)m_nodes.size() - 1));
		}
	}

	void FCTreeNode::collapse(){
		if (m_nodes.size() > 0){
			m_expended = false;
			collapseChildNodes(m_nodes, false);
		}
	}

	void FCTreeNode::collapseAll(){
		if (m_nodes.size() > 0){
			m_expended = false;
			collapseChildNodes(m_nodes, true);
		}
	}

	void FCTreeNode::expend(){
		if (m_nodes.size() > 0){
			m_expended = true;
			expendChildNodes(m_nodes, true, false);
		}
	}

	void FCTreeNode::expendAll(){
		if (m_nodes.size() > 0){
			m_expended = true;
			expendChildNodes(m_nodes, true, true);
		}
	}

	ArrayList<FCTreeNode*> FCTreeNode::getChildNodes(){
		return m_nodes;
	}

	int FCTreeNode::getNodeIndex(FCTreeNode *node){
		int size = (int)m_nodes.size();
		for(int i = 0; i < size; i++){
			if(m_nodes.get(i) == node){
				return i;
			}
		}
		return -1;
	}

	String FCTreeNode::getPaintText(){
		return getText();
	}

	void FCTreeNode::getProperty(const String& name, String *value, String *type){
		if(name == L"allowdragin"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(allowDragIn());
		}
		else if(name == L"allowdragout"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(allowDragOut());
		}
		else if (name == L"checked"){
			*type = L"bool";
            *value = FCStr::convertBoolToStr(isChecked());
        }
        else if (name == L"expended"){
			*type = L"bool";
            *value = FCStr::convertBoolToStr(isExpended());
        }
        else if (name == L"value"){
			*type = L"text";
            *value = getValue();
        }
		else{
			FCGridControlCell::getProperty(name, value, type);
		}
	}

	ArrayList<String> FCTreeNode::getPropertyNames(){
		ArrayList<String> propertyNames = FCGridControlCell::getPropertyNames();
		propertyNames.add(L"AllowDragIn");
		propertyNames.add(L"AllowDragOut");
		propertyNames.add(L"Checked");
		propertyNames.add(L"Expended");
		propertyNames.add(L"Value");
		return propertyNames;
	}

	String FCTreeNode::getString(){
		return m_text;
	}

	void FCTreeNode::insertNode(int index, FCTreeNode *node){
	    int rowIndex = -1;
        if (index == 0){
            if (node->getParent()){
                rowIndex = node->getParent()->getRow()->getIndex() + 1;
            }
			else{
				rowIndex = 0;
			}
        }
        else{
            if (m_nodes.size() > 0){
                rowIndex = m_nodes.get(index)->getRow()->getIndex();
            }
        }
		node->setParent(this);
        node->setTree(m_tree);
        node->onAddingNode(rowIndex);
        m_nodes.insert(index, node);
	}

	bool FCTreeNode::isNodeVisible(FCTreeNode *node){
	    FCTreeNode *parentNode = node->getParent();
        if (parentNode){
			if(!parentNode->isExpended()){
				return false;
			}
			else{
				return isNodeVisible(parentNode);
			}
        }
        else{
            return true;
        }
	}

	void FCTreeNode::onAddingNode(int index){
		FCGridRow *row = getRow();
		if(!row){
			row = new FCGridRow;
			FCTreeNode *parentNode = m_parent;
			if (!parentNode){
                if (index != -1){
					m_tree->insertRow(index, row);
					ArrayList<FCGridRow*> rows = m_tree->m_rows;
					int rowSize = (int)rows.size();
					for (int i = 0; i < rowSize; i++){
						rows.get(i)->setIndex(i);
					}
                }
                else{
                    m_tree->addRow(row);
					ArrayList<FCGridRow*> rows = m_tree->m_rows;
                    row->setIndex((int)rows.size() - 1);
                }
				row->addCell(0, this);
				m_targetColumn = m_tree->getColumn(0);
			}
			else{		
				int rowIndex = parentNode->getRow()->getIndex() + 1;
				if(index != -1){
					rowIndex = index;
				}
				else{
					FCTreeNode *lastNode = getLastNode(parentNode->getChildNodes());
					if (lastNode){
						if(!lastNode->getRow()){
							return;
						}
						rowIndex = lastNode->getRow()->getIndex() + 1;
					}
				}
				m_tree->insertRow(rowIndex, row);
				ArrayList<FCGridRow*> rows = m_tree->m_rows;
				if (rowIndex == (int)rows.size() - 1){
					row->setIndex(rowIndex);
				}
				else{
					int rowSize = (int)rows.size();
					for (int i = 0; i < rowSize; i++){
						rows.get(i)->setIndex(i);
					}
				}
				row->addCell(0, this);
				m_targetColumn = m_tree->getColumn(parentNode->getTargetColumn()->getIndex() + 1);
			}
			setColSpan((int)m_tree->m_columns.size());
			int nodeSize = (int)m_nodes.size();
			if (nodeSize > 0){
				for (int i = 0; i < nodeSize; i++){
					m_nodes.get(i)->onAddingNode(-1);
				}
			}
			row->setVisible(isNodeVisible(this));
		}
	}

	void FCTreeNode::onPaintCheckBox(FCPaint *paint, const FCRect& rect){
	    if (m_checked){
			if(m_tree->getCheckedImage().length() > 0){
				paint->drawImage(m_tree->getCheckedImage().c_str(), rect);
			}
			else{
				paint->fillRect(FCColor::argb(0, 0, 0), rect);
			}
        }
        else{
			if(m_tree->getUnCheckedImage().length() > 0){
				paint->drawImage(m_tree->getUnCheckedImage().c_str(), rect);
			}
			else{
				paint->drawRect(FCColor::argb(0, 0, 0), 1, 0, rect);
			}
        }
	}

	void FCTreeNode::onPaintNode(FCPaint *paint, const FCRect& rect)
	{
	    if (m_expended){
            if (m_tree->getExpendedNodeImage().length() > 0){
                paint->drawImage(m_tree->getExpendedNodeImage().c_str(), rect);
				return;
            }
        }
        else{
			if (m_tree->getCollapsedNodeImage().length() > 0){
                paint->drawImage(m_tree->getCollapsedNodeImage().c_str(), rect);
				return;
            }
        }
		int width = rect.right - rect.left;
        int height = rect.bottom - rect.top;
        FCPoint points[3];
		FCPoint point1 = {0}, point2 = {0}, point3 = {0};
        if (m_expended){
			point1.x = rect.left;
            point1.y = rect.top;
            point2.x = rect.left + width;
			point2.y = rect.top;
            point3.x = rect.left + width / 2;
			point3.y = rect.top + height;
        }
        else{
            point1.x = rect.left;
			point1.y = rect.top;
            point2.x = rect.left;
			point2.y = rect.top + height;
            point3.x = rect.left + width;
			point3.y = rect.top + height / 2;
        }
		points[0] = point1;
		points[1] = point2;
		points[2] = point3;
		FCGrid *grid = getGrid();
        paint->fillPolygon(grid->getTextColor(), points, 3);
	}

	void FCTreeNode::onPaint(FCPaint *paint, const FCRect& rect, const FCRect& clipRect, bool isAlternate){
		int clipW = clipRect.right - clipRect.left;
        int clipH = clipRect.bottom - clipRect.top;
		FCGrid *grid = getGrid();
		FCGridColumn *column = getColumn();
		FCGridRow *row = getRow();
		if(clipW > 0 && clipH > 0 && grid && column && row){
			bool selected = false;
			ArrayList<FCGridRow*> selectedRows = grid->getSelectedRows();
			int selectedRowsSize = (int)selectedRows.size();
			for(int i = 0; i < selectedRowsSize; i++){
				if(selectedRows.get(i) == row){
					selected = true;
					break;
				}
			}
			int height = rect.bottom - rect.top;
			FCFont *font = 0;
            Long backColor = FCColor_None;
            Long textColor = FCColor_None;
			bool autoEllipsis = m_tree->autoEllipsis();
			FCGridCellStyle *style = getStyle();
            if (style){
				if (style->autoEllipsis()){
                    autoEllipsis = style->autoEllipsis();
                }
                backColor = style->getBackColor();
                if (style->getFont()){
                    font = style->getFont();
                }
                textColor = style->getTextColor();
            }
            FCGridRowStyle *rowStyle = grid->getRowStyle();
            if (isAlternate){
                FCGridRowStyle *alternateRowStyle = grid->getAlternateRowStyle();
                if (alternateRowStyle){
                    rowStyle = alternateRowStyle;
                }
            }
			if(rowStyle){
				if (backColor == FCColor_None){
					if (selected){
						backColor = rowStyle->getSelectedBackColor();
					}
					else if (row == grid->getHoveredRow()){
						backColor = rowStyle->getHoveredBackColor();
					}
					else{
						backColor = rowStyle->getBackColor();
					}
				}
				if(!font){
					font = rowStyle->getFont();
				}
				if (textColor == FCColor_None){
					if (selected){
						textColor = rowStyle->getSelectedTextColor();
					}
					else if (row == grid->getHoveredRow()){
						textColor = rowStyle->getHoveredTextColor();
					}
					else{
						textColor = rowStyle->getTextColor();
					}
				}
			}
			paint->fillRect(backColor, rect);
			int scrollH = 0;
			FCHScrollBar *hscrollBar = getGrid()->getHScrollBar();
			if (hscrollBar && hscrollBar->isVisible()){
				scrollH = hscrollBar->getPos();
			}
			FCRect headerRect = m_targetColumn->getBounds();
			headerRect.left += grid->getHorizontalOffset() - scrollH;
			headerRect.top += grid->getVerticalOffset() - scrollH;
			int left = headerRect.left;
			if(m_tree->hasCheckBoxes()){
			    int cw = m_tree->getCheckBoxSize().cx;
                int ch = m_tree->getCheckBoxSize().cy;
				FCRect checkBoxRect = {0};
                checkBoxRect.left = left;
                checkBoxRect.top = rect.top + (height - ch) / 2;
                checkBoxRect.right = checkBoxRect.left + cw;
                checkBoxRect.bottom = checkBoxRect.top + ch;
                onPaintCheckBox(paint, checkBoxRect);
                left += cw + 10;
			}
			int nw = m_tree->getNodeSize().cx;
            int nh = m_tree->getNodeSize().cy;
			if (m_nodes.size() > 0){
				FCRect nodeRect = {0};
                nodeRect.left = left;
                nodeRect.top = rect.top + (height - nh) / 2;
                nodeRect.right = nodeRect.left + nw;
                nodeRect.bottom = nodeRect.top + nh;
                onPaintNode(paint, nodeRect);
			}
			left += nw + 10;
			m_indent = left;
			String text = getPaintText();
			if (text.length() > 0){
				FCSize tSize = paint->textSize(text.c_str(), font);
				FCRect tRect = {0};
				tRect.left = left;
				tRect.top = rect.top + (row->getHeight() - tSize.cy) / 2;
				tRect.right = tRect.left + tSize.cx;
				tRect.bottom = tRect.top + tSize.cy;
				if(autoEllipsis && (tRect.right < clipRect.right || tRect.bottom < clipRect.bottom)){
                    if(tRect.right < clipRect.right){
                        tRect.right = clipRect.right;
                    }
                    if(tRect.bottom < clipRect.bottom){
                        tRect.bottom = clipRect.bottom;
                    }
					paint->drawTextAutoEllipsis(text.c_str(), textColor, font, tRect);
				}
				else{
					paint->drawText(text.c_str(), textColor, font, tRect);
				}
			}
		}
		onPaintControl(paint, rect, clipRect);
	}

	void FCTreeNode::onRemovingNode(){
		m_indent = 0;
		FCGridRow *row = getRow();
		if(row){
			int nodeSize = (int)m_nodes.size();
			if (nodeSize > 0){
                for (int i = 0; i < nodeSize; i++){
                    m_nodes.get(i)->onRemovingNode();
                }
            }
			m_tree->removeRow(row);
			row->removeCell(0);
			delete row;
			row = 0;
			setRow(0);
			ArrayList<FCGridRow*> rows = m_tree->m_rows;
			int rowSize = (int)rows.size();
			for (int i = 0; i < rowSize; i++){
				rows.get(i)->setIndex(i);
			}
		}
	}

	void FCTreeNode::removeNode(FCTreeNode *node){
		for(int i = 0; i < m_nodes.size(); i++){
			FCTreeNode *tn = m_nodes.get(i);
			if(tn == node){
				node->onRemovingNode();
				m_nodes.removeAt(i);
				break;
			}
		}
	}

	void FCTreeNode::setProperty(const String& name, const String& value){
		if(name == L"allowdragin"){
			setAllowDragIn(FCStr::convertStrToBool(value));
		}
		else if(name == L"allowdragout"){
			setAllowDragOut(FCStr::convertStrToBool(value));
		}
	    else if (name == L"checked"){
            setChecked(FCStr::convertStrToBool(value));
        }
        else if (name == L"expended"){
            setExpended(FCStr::convertStrToBool(value));
        }
        else if (name == L"value"){
            setValue(value);
        }
		else{
			FCGridControlCell::setProperty(name, value);
		}
	}

	void FCTreeNode::setString(const String& value){
		m_text = value;
	}
}