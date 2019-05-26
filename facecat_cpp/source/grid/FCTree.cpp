#include "..\\..\\stdafx.h"
#include "..\\..\\include\\grid\\FCTree.h"

namespace FaceCat{
	FCTree::FCTree(){
		setGridLineColor(FCColor_None);
		m_checkBoxes = false;
		m_checkBoxSize.cx = 14;
		m_checkBoxSize.cy = 14;
		m_movingNode = 0;
		m_nodeSize.cx = 14;
		m_nodeSize.cy = 14;
	}

	FCTree::~FCTree(){
		m_movingNode = 0;
		m_nodes.clear();
	}

	bool FCTree::hasCheckBoxes(){
		return m_checkBoxes;
	}

	void FCTree::setCheckBoxes(bool checkBoxes){
		m_checkBoxes = checkBoxes;
	}

	FCSize FCTree::getCheckBoxSize(){
		return m_checkBoxSize;
	}

	void FCTree::setCheckBoxSize(FCSize checkBoxSize){
		m_checkBoxSize = checkBoxSize;
	}

	String FCTree::getCheckedImage(){
		return m_checkedImage;
	}

	void FCTree::setCheckedImage(const String& checkedImage){
		m_checkedImage = checkedImage;
	}

	String FCTree::getCollapsedNodeImage(){
		return m_collapsedNodeImage;
	}

	void FCTree::setCollapsedNodeImage(const String& collapsedNodeImage){
		m_collapsedNodeImage = collapsedNodeImage;
	}

	String FCTree::getExpendedNodeImage(){
		return m_expendedNodeImage;
	}

	void FCTree::setExpendedNodeImage(const String& expendedNodeImage){
		m_expendedNodeImage = expendedNodeImage;
	}

	FCSize FCTree::getNodeSize(){
		return m_nodeSize;
	}

	void FCTree::setNodeSize(FCSize nodeSize){
		m_nodeSize = nodeSize;
	}

	ArrayList<FCTreeNode*> FCTree::getSelectedNodes(){
		ArrayList<FCTreeNode*> selectedNodes;
		ArrayList<FCGridRow*> selectedRows = getSelectedRows();
		int selectedRowsSize = (int)selectedRows.size();
		for(int i = 0; i < selectedRowsSize; i++){
			ArrayList<FCGridCell*> cells = selectedRows.get(i)->m_cells;
            int cellsSize = (int)cells.size();
            for (int j = 0; j < cellsSize; j++){
                FCTreeNode *node = dynamic_cast<FCTreeNode*>(cells.get(j));
                if (node){
                    selectedNodes.add(node);
                    break;
                }
            }
		}
		return selectedNodes;
	}

	void FCTree::setSelectedNodes(ArrayList<FCTreeNode*> selectedNodes){
		int selectedNodesSize = (int)selectedNodes.size();
		ArrayList<FCGridRow*> selectedRows;
		for(int i = 0; i < selectedNodesSize; i++){
			selectedRows.add(selectedNodes.get(i)->getRow());
		}
		setSelectedRows(selectedRows);
	}

	String FCTree::getUnCheckedImage(){
		return m_unCheckedImage;
	}

	void FCTree::setUnCheckedImage(const String& unCheckedImage){
		m_unCheckedImage = unCheckedImage;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void FCTree::appendNode(FCTreeNode *node){
		node->setTree(this);
		node->onAddingNode(-1);
		m_nodes.add(node);
	}

	void FCTree::clearNodes(){
		while((int)m_nodes.size() > 0){
			removeNode(m_nodes.get((int)m_nodes.size() - 1));
		}
	}

	void FCTree::collapse(){
		int nodesSize = (int)m_nodes.size();
		if (nodesSize > 0){
			for(int i = 0; i < nodesSize; i++){
				m_nodes.get(i)->collapse();
			}
		}
	}

	void FCTree::collapseAll(){
		int nodesSize = (int)m_nodes.size();
		if (nodesSize > 0){
			for (int i = 0; i < nodesSize; i++){
				m_nodes.get(i)->collapseAll();
			}
		}
	}

	void FCTree::expend(){
		int nodesSize = (int)m_nodes.size();
		if (nodesSize > 0){
			for(int i = 0; i < nodesSize; i++){
				m_nodes.get(i)->expend();
			}
		}
	}

	void FCTree::expendAll()
	{
		int nodesSize = (int)m_nodes.size();
		if (nodesSize > 0){
			for (int i = 0; i < nodesSize; i++){
				m_nodes.get(i)->expendAll();
			}
		}
	}

	ArrayList<FCTreeNode*> FCTree::getChildNodes(){
		return m_nodes;
	}

	String FCTree::getControlType(){
		return L"Tree";
	}

	int FCTree::getNodeIndex(FCTreeNode *node){
		int size = (int)m_nodes.size();
		for(int i = 0; i < size; i++){
			if(m_nodes.get(i) == node){
				return i;
			}
		}
		return -1;
	}

	void FCTree::getProperty(const String& name, String *value, String *type){
		if(name == L"checkboxes"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(hasCheckBoxes());
		}
		else if(name == L"checkboxsize"){
			*type = L"size";
			*value = FCStr::convertSizeToStr(getCheckBoxSize());
		}
		else if (name == L"checkedimage"){
            *type = L"text";
            *value = getCheckedImage();
        }
        else if (name == L"collapsednodeimage"){
            *type = L"text";
            *value = getCollapsedNodeImage();
        }
        else if (name == L"expendednodeimage"){
            *type = L"text";
            *value = getExpendedNodeImage();
        }
        else if (name == L"uncheckedimage") {
            *type = L"text";
            *value = getUnCheckedImage();
        }
		else if(name == L"nodesize"){
			*type = L"size";
			*value = FCStr::convertSizeToStr(getNodeSize());
		}
		else{
			FCGrid::getProperty(name, value, type);
		}
	}

	ArrayList<String> FCTree::getPropertyNames(){
		ArrayList<String> propertyNames = FCGrid::getPropertyNames();
		propertyNames.add(L"CheckBoxes");
		propertyNames.add(L"CheckBoxSize");
		propertyNames.add(L"CheckedImage");
		propertyNames.add(L"CollapsedNodeImage");
		propertyNames.add(L"ExpendedNodeImage");
		propertyNames.add(L"UnCheckedImage");
		propertyNames.add(L"NodeSize");
		return propertyNames;
	}

	void FCTree::insertNode(int index, FCTreeNode *node){
	    int rowIndex = -1;
        if (index == 0){
            if (node->getParent()){
                rowIndex = node->getParent()->getRow()->getIndex() + 1;
            }
			rowIndex = 0;
        }
        else{
            if (m_nodes.size() > 0){
                rowIndex = m_nodes.get(index)->getRow()->getIndex();
            }
        }
        node->setTree(this);
        node->onAddingNode(rowIndex);
        m_nodes.insert(index, node);
	}

	void FCTree::onCellTouchDown(FCGridCell *cell, FCTouchInfo touchInfo){
		FCGrid::onCellTouchDown(cell, touchInfo);
		FCPoint mp = touchInfo.m_firstPoint;
		FCTreeNode *node = dynamic_cast<FCTreeNode*>(cell);
        if (node){
            int scrollH = 0;
            FCHScrollBar *hscrollBar = getHScrollBar();
            if (hscrollBar && hscrollBar->isVisible()){
                scrollH = hscrollBar->getPos();
            }
            FCRect headerRect = node->getTargetColumn()->getBounds();
            headerRect.left += getHorizontalOffset() - scrollH;
            headerRect.top += getVerticalOffset() - scrollH;
            int left = headerRect.left;
            if (m_checkBoxes){
                int cw = m_checkBoxSize.cx;
                if (mp.x >= left && mp.x <= left + cw){
                    node->setChecked(!node->isChecked());
                    return;
                }
                left += cw + 10;
            }
            ArrayList<FCTreeNode*> childNodes = node->getChildNodes();
            if (childNodes.size() > 0){
                int nw = m_nodeSize.cx;
                if (mp.x >= left && mp.x <= left + nw){
                    if (node->isExpended()){
                        node->collapse();
                    }
                    else{
                        node->expend();
                    }
                    update();
                    return;
                }
            }
            if (node->allowDragOut()){
                m_movingNode = node;
            }
        }
	}

	void FCTree::onCellTouchMove(FCGridCell *cell, FCTouchInfo touchInfo){
		FCGrid::onCellTouchMove(cell, touchInfo);
		if(m_movingNode){
			invalidate();
		}
	}

	void FCTree::onCellTouchUp(FCGridCell *cell, FCTouchInfo touchInfo){
		FCGrid::onCellTouchUp(cell, touchInfo);
		FCPoint mp = touchInfo.m_firstPoint;
		if (m_movingNode){
            FCGridRow *curRow = getRow(mp);
            if (curRow){
				FCTreeNode *curNode = dynamic_cast<FCTreeNode*>(curRow->getCell(0));
                if (curNode->allowDragIn() && m_movingNode != curNode){
					FCTreeNode *curNodeParent = curNode->getParent();
					FCTreeNode *movingNodeParent = m_movingNode->getParent();
					if(movingNodeParent){
						movingNodeParent->removeNode(m_movingNode);
					}
					else{
						removeNode(m_movingNode);
					}
                    if (curNodeParent){
                        
                        if (curNodeParent == movingNodeParent){
                            curNodeParent->insertNode(curNodeParent->getNodeIndex(curNode), m_movingNode);
                        }
                        else{
                            curNode->appendNode(m_movingNode);
                        }
                    }
                    else{                      
                        if (curNodeParent == movingNodeParent){
                            insertNode(getNodeIndex(curNode), m_movingNode);
                        }
                        else{
                            curNode->appendNode(m_movingNode);
                        }
                    }
                    curNode->expend();
                }
            }
            m_movingNode = 0;
			update();
        }
	}

	void FCTree::onPaintForeground(FCPaint *paint, const FCRect& clipRect){
		FCGrid::onPaintForeground(paint, clipRect);
		if (m_movingNode){
			FCFont *font = getFont();
            FCPoint mp = getTouchPoint();
			FCSize tSize = paint->textSize(m_movingNode->getText().c_str(), font);
			FCRect tRect = {mp.x, mp.y, mp.x + tSize.cx, mp.y + tSize.cy};
            paint->drawText(m_movingNode->getText().c_str(), getTextColor(), font, tRect);
        }
	}

	void FCTree::onPaintEditTextBox(FCGridCell *cell, FCPaint *paint, const FCRect& rect, const FCRect& clipRect){
		FCTextBox *editTextBox = getEditTextBox();
        if (editTextBox){
            FCTreeNode *node = dynamic_cast<FCTreeNode*>(cell);
            if (node){
                int indent = node->getIndent();
				FCRect newRect = rect;
                newRect.left += indent;
                if (newRect.right < newRect.left){
                    newRect.right = newRect.left;
                }
                editTextBox->setBounds(newRect);
                editTextBox->setDisplayOffset(false);
                editTextBox->bringToFront();
            }
			else{
				FCGrid::onPaintEditTextBox(cell, paint, rect, clipRect);
			}
        }
	}

	void FCTree::removeNode(FCTreeNode *node){
		node->onRemovingNode();
		for(int i = 0; i < m_nodes.size(); i++){
			FCTreeNode *tn = m_nodes.get(i);
			if(tn == node){
				m_nodes.removeAt(i);
				break;
			}
		}
	}

	void FCTree::setProperty(const String& name, const String& value){
		if(name == L"checkboxes"){
			setCheckBoxes(FCStr::convertStrToBool(value));
		}
		else if(name == L"checkboxsize"){
			setCheckBoxSize(FCStr::convertStrToSize(value));
		}
	    else if (name == L"checkedimage"){
            setCheckedImage(value);
        }
        else if (name == L"collapsednodeimage"){
            setCollapsedNodeImage(value);
        }
        else if (name == L"expendednodeimage"){
            setExpendedNodeImage(value);
        }
        else if (name == L"uncheckedimage"){
            setUnCheckedImage(value);
        }
		else if(name == L"nodesize"){
			setNodeSize(FCStr::convertStrToSize(value));
		}
		else{
			FCGrid::setProperty(name, value);
		}
	}
}