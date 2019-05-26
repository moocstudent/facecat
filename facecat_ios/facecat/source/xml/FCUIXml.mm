#include "stdafx.h"
#include "FCUIXml.h"

namespace FaceCat{
	void FCUIXml::createBandedGridBands(xmlNodePtr node, FCView *control){
		FCBandedGrid *bandedGridA = dynamic_cast<FCBandedGrid*>(control);
        xmlNodePtr spChildNode = node->children;
		while (spChildNode){
            String subNodeName = FCStr::stringTowstring((char*)spChildNode->name);
			FCView *subControl = createControl(spChildNode, FCStr::toLower(subNodeName));
			FCGridBand *band = dynamic_cast<FCGridBand*>(subControl);
			if(band){
				band->setNative(m_native);
				bandedGridA->addBand(band);
			}
			setAttributesBefore(spChildNode, band);
			createBandedGridBands(spChildNode, band);
			setAttributesAfter(spChildNode, band);
			onAddControl(band, spChildNode);
            spChildNode = spChildNode->next;
        }		
	}
	
	void FCUIXml::createBandedGridColumns(xmlNodePtr node, FCView *control){
		FCGridBand *gridBand = dynamic_cast<FCGridBand*>(control);
        xmlNodePtr spChildNode = node->children;
		while (spChildNode){
            String subNodeName = FCStr::stringTowstring((char*)spChildNode->name);
			FCView *subControl = createControl(spChildNode, FCStr::toLower(subNodeName));
			FCGridBand *band = dynamic_cast<FCGridBand*>(subControl);
			if(band){
				band->setNative(m_native);
				gridBand->addBand(band);
				setAttributesBefore(spChildNode, band);
				createBandedGridBands(spChildNode, band);
				setAttributesAfter(spChildNode, band);
				onAddControl(band, spChildNode);
			}
			FCBandedGridColumn *bandcolumn = dynamic_cast<FCBandedGridColumn*>(subControl);
			if(bandcolumn){
				bandcolumn->setNative(m_native);
				gridBand->addColumn(bandcolumn);
				setAttributesBefore(spChildNode, bandcolumn);
				setAttributesAfter(spChildNode, bandcolumn);
				onAddControl(bandcolumn, spChildNode);
			}
            spChildNode = spChildNode->next;
        }
	}
	
    void FCUIXml::createMenuItem(xmlNodePtr node, FCMenu *menu, FCMenuItem *parentItem){
		FCMenuItem *item = new FCMenuItem;
		item->setNative(m_native);
		setAttributesBefore(node, item);
		if (parentItem){
			parentItem->addItem(item);
		}
		else{
			menu->addItem(item);
		}
        xmlNodePtr spChildNode = node->children;
        while (spChildNode){
            createMenuItem(spChildNode, menu, item);
            spChildNode = spChildNode->next;
        }
		setAttributesAfter(node, item);
		onAddControl(item, node);
	}
    
	void FCUIXml::createGridColumns(xmlNodePtr node, FCView *control)
	{
		FCGrid *grid = dynamic_cast<FCGrid*>(control);
        xmlNodePtr spChildNode = node->children;
        while (spChildNode){
            String subNodeName = FCStr::stringTowstring((char*)spChildNode->name);
			FCView *subControl = createControl(spChildNode, FCStr::toLower(subNodeName));
			FCGridColumn *column = dynamic_cast<FCGridColumn*>(subControl);
			if(column){
				column->setNative(m_native);
				grid->addColumn(column);
			}
			setAttributesBefore(spChildNode, column);
			readChildNodes(spChildNode, column);
			setAttributesAfter(spChildNode, column);
			onAddControl(column, spChildNode);
			String subNodeValue = L"";
            if(spChildNode->content){
                subNodeValue = FCStr::stringTowstring((char*)spChildNode->content);
            }
            else{
                if(spChildNode->children){
                    if(spChildNode->children->content){
                        subNodeValue = FCStr::stringTowstring((char*)spChildNode->children->content);
                    }
                }
            }
            if ((int)subNodeValue.length() > 0){
                column->setText(subNodeValue);
            }
            spChildNode = spChildNode->next;
        }
	}
	
	void FCUIXml::createGridRow(xmlNodePtr node, FCView *control){
		FCGrid *grid = dynamic_cast<FCGrid*>(control);
		FCGridRow *row = new FCGridRow;
		grid->addRow(row);
		setAttributesBefore(node, row);
        int col = 0;
        xmlNodePtr spChildNode2 = node->children;
        while (spChildNode2){
            String subNodeName = FCStr::stringTowstring((char*)spChildNode2->name);
			String subNodeValue = L"";
            if(spChildNode2->content){
                subNodeName = FCStr::stringTowstring((char*)spChildNode2->content);
            }
            else{
                if(spChildNode2->children){
                    if(spChildNode2->children->content){
                        subNodeValue = FCStr::stringTowstring((char*)spChildNode2->children->content);
                    }
                }
            }
			if(subNodeName == L"cell" || subNodeName == L"td"){
				String cellType = L"string";
				HashMap<String, String> attributes = getAttributes(spChildNode2);
				if (attributes.containsKey(L"type")){
					cellType = attributes.get(L"type");
				}
				FCGridCell *cell = 0;
				if (cellType == L"bool"){
					cell = new FCGridBoolCell;
				}
				else if (cellType == L"button"){
					cell = new FCGridButtonCell;
				}
				else if (cellType == L"checkbox"){
					cell = new FCGridCheckBoxCell;
				}
				else if (cellType == L"double"){
					cell = new FCGridDoubleCell;
                }
				else if (cellType == L"float"){
					cell = new FCGridFloatCell;
				}
				else if (cellType == L"string"){
					cell = new FCGridStringCell;
				}
				else if (cellType == L"int"){
					cell = new FCGridIntCell;
				}
				else if (cellType == L"long"){
					cell = new FCGridLongCell;
				}
				else if (cellType == L"textbox"){
					cell = new FCGridTextBoxCell;
				}
				row->addCell(col, cell);
				setAttributesBefore(spChildNode2, cell);
				cell->setString(subNodeValue);
				setAttributesAfter(spChildNode2, cell);
				col++;
			}
            spChildNode2 = spChildNode2->next;
        }
		setAttributesAfter(node, row);
	}
    
	void FCUIXml::createGridRows(xmlNodePtr node, FCView *control){
        xmlNodePtr spChildNode = node->children;
        while (spChildNode){
            String nodeName = FCStr::stringTowstring((char*)spChildNode->name);
			if(nodeName == L"row" || nodeName == L"tr"){
				createGridRow(spChildNode, control);
			}
            spChildNode = spChildNode->next;
        }
	}
    
	void FCUIXml::createSplitLayoutSubProperty(xmlNodePtr node, FCSplitLayoutDiv *splitLayoutDiv){
        setAttributesBefore(node, splitLayoutDiv);
		int oldCount = (int)splitLayoutDiv->m_controls.size();
        readChildNodes(node, splitLayoutDiv);
		ArrayList<FCView*> newControls = splitLayoutDiv->m_controls;
		int newCount = (int)newControls.size();
        if (newCount - oldCount >= 2){
            splitLayoutDiv->setFirstControl(newControls.get(newCount - 2));
            splitLayoutDiv->setSecondControl(newControls.get(newCount - 1));
        }
		setAttributesAfter(node, splitLayoutDiv);
        splitLayoutDiv->update();
		onAddControl(splitLayoutDiv, node);
	}
	
	void FCUIXml::createTableLayoutSubProperty(xmlNodePtr node, FCTableLayoutDiv *tableLayoutDiv ){
		setAttributesBefore(node, tableLayoutDiv);
        xmlNodePtr spChildNode = node->children;
		while (spChildNode){
            String subNodeName = FCStr::stringTowstring((char*)spChildNode->name);
			if(subNodeName == L"columnstyles"){
				xmlNodePtr spChildNode1 = spChildNode->children;
				while(spChildNode1){
					String subNodeName1 = FCStr::stringTowstring((char*)spChildNode1->name);
					if (subNodeName1 == L"columnstyle"){
                        FCColumnStyle column(FCSizeType_PercentSize, 0.0f);
						HashMap<String, String> mp = getAttributes(spChildNode1);
                        for (int c = 0; c < mp.size(); c++){
                            column.setProperty(mp.getKey(c), mp.getValue(c));
						}
                        tableLayoutDiv->m_columnStyles.add(column);
					}					
					spChildNode1 = spChildNode1->next;
				}
			}
			else if(subNodeName == L"rowstyles"){
				xmlNodePtr spChildNode1 = spChildNode->children;
				while(spChildNode1){
					String subNodeName1 = FCStr::stringTowstring((char*)spChildNode1->name);
					if (subNodeName1 == L"rowstyle"){
                        FCRowStyle row(FCSizeType_PercentSize, 0.0f);
						HashMap<String, String> mp = getAttributes(spChildNode1);
						for (int c = 0; c < mp.size(); c++){
                            row.setProperty(mp.getKey(c), mp.getValue(c));
						}
                        tableLayoutDiv->m_rowStyles.add(row);
					}					
					spChildNode1 = spChildNode1->next;
				}
			}
			else if (subNodeName == L"childs"){
				readChildNodes(spChildNode, tableLayoutDiv);
			}
            spChildNode = spChildNode->next;
		}
		setAttributesAfter(node, tableLayoutDiv);
		tableLayoutDiv->update();
		onAddControl(tableLayoutDiv, node);
	}
    
	void FCUIXml::createTabPage(xmlNodePtr node, FCView *control){
		FCTabControl *tabControl = dynamic_cast<FCTabControl*>(control);
		if(tabControl){
			FCTabPage *tabPage = new FCTabPage;
			tabPage->setNative(m_native);
			tabControl->addControl(tabPage);
			setAttributesBefore(node, tabPage);
			readChildNodes(node, tabPage);
			setAttributesAfter(node, tabPage);
			onAddControl(tabPage, node);
		}
	}
    
    void FCUIXml::createTreeNode(xmlNodePtr node, FCView *control, FCTreeNode *treeNode){
	    FCTree *tree = dynamic_cast<FCTree*>(control);
        if (tree){
			FCTreeNode *appendNode = new FCTreeNode;
            if (treeNode){
				treeNode->appendNode(appendNode);
            }
            else{
				tree->appendNode(appendNode);
            }
            setAttributesBefore(node, appendNode);
            xmlNodePtr spChildNode = node->children;
            while (spChildNode){
                String nodeName = FCStr::stringTowstring((char*)spChildNode->name);
                if(nodeName == L"node"){
					createTreeNode(spChildNode, control, appendNode);
				}
                spChildNode = spChildNode->next;
            }
			setAttributesAfter(node, appendNode);
        }
	}
    
	void FCUIXml::createTreeNodes(xmlNodePtr node, FCView *control){
        xmlNodePtr spChildNode = node->children;
        while (spChildNode){
            createTreeNode(spChildNode, control, 0);
            spChildNode = spChildNode->next;
        }
	}
    
	FCView* FCUIXml::createUserControl(xmlNodePtr node){
		FCView *userControl = 0;
        HashMap<String, String> attributes = getAttributes(node);
		if (attributes.containsKey(L"cid")){
            userControl = createControl(node, attributes.get(L"cid"));
        }
        if (userControl){
			userControl->setNative(m_native);
            return userControl;
        }
        else{
            return new FCButton;
        }
	}
    
	///////////////////////////////////////////////////////////////////////////////////////////////////////
    
	FCUIXml::FCUIXml(){
		m_event = new FCUIEvent(this);
		m_native = 0;
		m_script = new FCUIScript(this);
	}
    
	FCUIXml::~FCUIXml(){
		m_controls.clear();
		if(m_event){
			delete m_event;
			m_event = 0;
		}
		m_native = 0;
		if(m_script){
			delete m_script;
			m_script = 0;
		}
		m_styles.clear();
	}
    
	FCUIEvent* FCUIXml::getEvent(){
		return m_event;
	}
    
	void FCUIXml::setEvent(FCUIEvent *uiEvent){
		m_event = uiEvent;
	}
    
	FCNative* FCUIXml::getNative(){
		return m_native;
	}
    
	void FCUIXml::setNative(FCNative *native){
		m_native = native;
	}
    
	FCUIScript* FCUIXml::getScript(){
		return m_script;
	}
    
	void FCUIXml::setScript(FCUIScript *script){
		m_script = script;
	}
    
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
	FCButton* FCUIXml::getButton(const String& name){
		FCView *control = findControl(name);
        if (control){
            return dynamic_cast<FCButton*>(control);
        }
        return 0;
	}
    
	FCCheckBox* FCUIXml::getCheckBox(const String& name){
		FCView *control = findControl(name);
        if (control){
            return dynamic_cast<FCCheckBox*>(control);
        }
        return 0;
	}
    
    FCComboBox* FCUIXml::getComboBox(const String& name){
		FCView *control = findControl(name);
        if (control){
            return dynamic_cast<FCComboBox*>(control);
        }
        return 0;
	}
    
    FCDiv* FCUIXml::getDiv(const String& name){
		FCView *control = findControl(name);
        if (control){
            return dynamic_cast<FCDiv*>(control);
        }
        return 0;
	}
    
	FCGrid* FCUIXml::getGrid(const String& name){
		FCView *control = findControl(name);
        if (control){
            return dynamic_cast<FCGrid*>(control);
        }
        return 0;
	}
    
	FCGroupBox* FCUIXml::getGroupBox(const String& name){
		FCView *control = findControl(name);
        if (control){
            return dynamic_cast<FCGroupBox*>(control);
        }
        return 0;
	}
    
	FCLabel* FCUIXml::getLabel(const String& name){
		FCView *control = findControl(name);
        if (control){
            return dynamic_cast<FCLabel*>(control);
        }
        return 0;
	}
    
	FCLayoutDiv* FCUIXml::getLayoutDiv(const String& name){
		FCView *control = findControl(name);
        if (control){
            return dynamic_cast<FCLayoutDiv*>(control);
        }
        return 0;
	}
    
	ArrayList<FCView*> FCUIXml::getLikeControls(const String& name)
	{
		ArrayList<FCView*> controls;
        for(int c = 0; c < m_controls.size(); c++){
			FCView *control = m_controls.get(c);
			if(control->getName().find(name) != -1){
				controls.add(control);
			}
		}
        return controls;
	}
    
    FCMenu* FCUIXml::getMenu(const String& name){
		FCView *control = findControl(name);
        if (control){
            return dynamic_cast<FCMenu*>(control);
        }
        return 0;
	}
    
	FCMenuItem* FCUIXml::getMenuItem(const String& name){
		FCView *control = findControl(name);
        if (control){
            return dynamic_cast<FCMenuItem*>(control);
        }
        return 0;
	}
                                        
	FCRadioButton* FCUIXml::getRadioButton(const String& name){
		FCView *control = findControl(name);
        if (control){
            return dynamic_cast<FCRadioButton*>(control);
        }
        return 0;
	}
    
    FCSpin* FCUIXml::getSpin(const String& name){
		FCView *control = findControl(name);
        if (control){
            return dynamic_cast<FCSpin*>(control);
        }
        return 0;
	}
    
	FCSplitLayoutDiv* FCUIXml::getSplitLayoutDiv(const String& name){
		FCView *control = findControl(name);
        if (control){
            return dynamic_cast<FCSplitLayoutDiv*>(control);
        }
        return 0;
	}
    
	FCTabControl* FCUIXml::getTabControl(const String& name){
		FCView *control = findControl(name);
        if (control){
            return dynamic_cast<FCTabControl*>(control);
        }
        return 0;
	}
    
	FCTableLayoutDiv* FCUIXml::getTableLayoutDiv(const String& name){
		FCView *control = findControl(name);
        if (control){
            return dynamic_cast<FCTableLayoutDiv*>(control);
        }
        return 0;
	}
    
	FCTabPage* FCUIXml::getTabPage(const String& name){
		FCView *control = findControl(name);
        if (control){
            return dynamic_cast<FCTabPage*>(control);
        }
        return 0;
	}
    
	FCTextBox* FCUIXml::getTextBox(const String& name){
		FCView *control = findControl(name);
        if (control){
            return dynamic_cast<FCTextBox*>(control);
        }
        return 0;
	}
    
    FCTree* FCUIXml::getTree(const String& name){
		FCView *control = findControl(name);
        if (control){
            return dynamic_cast<FCTree*>(control);
        }
        return 0;
	}
    
	FCWindow* FCUIXml::getWindow(const String& name){
	    FCView *control = findControl(name);
        if (control){
            return dynamic_cast<FCWindow*>(control);
        }
        return 0;
	}
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
	bool FCUIXml::containsControl(FCView *control){
        for(int c = 0; c < m_controls.size(); c++){
			if(control == m_controls.get(c)){
				return true;
			}
		}
		return false;
	}
    
	FCView* FCUIXml::createControl(xmlNodePtr node, const String& type){
        if(type == L"band"){
            return new FCGridBand;
        }
        else if(type == L"bandcolumn"){
            return new FCBandedGridColumn;
        }
        else if(type == L"bandedgrid"){
            return new FCBandedGrid;
        }
        else if (type == L"button"){
            return new FCButton;
        }
        else if (type == L"calendar"){
            return new FCCalendar;
        }
        else if (type == L"chart"){
            if (m_native){
                return new FCChart;
            }
            else{
                return 0;
            }
        }
        else if (type == L"checkbox"){
            return new FCCheckBox;
        }
        else if (type == L"column" || type == L"th"){
            return new FCGridColumn;
        }
        else if (type == L"combobox" || type == L"select"){
            return new FCComboBox;
        }
        else if (type == L"datepicker"){
            return new FCDateTimePicker;
        }
        else if(type == L"div"){
            HashMap<String, String> attributes = getAttributes(node);
            if (attributes.containsKey(L"type")){
                String inputType = attributes.get(L"type");
                if (inputType == L"groupbox"){
                    return new FCGroupBox;
                }
                else if (inputType == L"layout"){
                    return new FCLayoutDiv;
                }
                else if (inputType == L"splitlayout"){
                    return new FCSplitLayoutDiv;
                }
                else if (inputType == L"tabcontrol"){
                    return new FCTabControl;
                }
                else if (inputType == L"tabpage"){
                    return new FCTabPage;
                }
                else if (inputType == L"tablelayout"){
                    return new FCTableLayoutDiv;
                }
                else if(inputType == L"usercontrol"){
                    return createUserControl(node);
                }
            }
            return new FCDiv;
        }
        else if (type == L"grid" || type == L"table"){
            return new FCGrid;
        }
        else if(type == L"groupbox"){
            return new FCGroupBox;
        }
        else if (type == L"input"){
            HashMap<String, String> attributes = getAttributes(node);
            if (attributes.containsKey(L"type")){
                String inputType = attributes.get(L"type");
                if (inputType == L"button"){
                    return new FCButton;
                }
                else if (inputType == L"checkbox"){
                    return new FCCheckBox;
                }
                else if (inputType == L"datetime"){
                    return new FCDateTimePicker;
                }
                else if (inputType == L"radio"){
                    return new FCRadioButton;
                }
                else if (inputType == L"range"){
                    return new FCSpin;
                }
                else if (inputType == L"text"){
                    return new FCTextBox;
                }
                else if(inputType == L"usercontrol"){
                    return createUserControl(node);
                }
            }
            attributes.clear();
        }
        else if (type == L"label"){
            return new FCLabel;
        }
        else if(type == L"layoutdiv"){
            return new FCLayoutDiv;
        }
        else if(type == L"linklabel"  || type == L"a"){
            return new FCLinkLabel;
        }
        else if(type == L"menu"){
            return new FCMenu;
        }
        else if (type == L"radiobutton"){
            return new FCRadioButton;
        }
        else if (type == L"spin"){
            return new FCSpin;
        }
        else if (type == L"splitlayoutdiv"){
            return new FCSplitLayoutDiv;
        }
        else if (type == L"tabcontrol"){
            return new FCTabControl;
        }
        else if(type == L"tablelayoutdiv"){
            return new FCTableLayoutDiv;
        }
        else if (type == L"textbox"){
            return new FCTextBox;
        }
        else if (type == L"tree"){
            return new FCTree;
        }
        else if(type == L"usercontrol"){
            return createUserControl(node);
        }
        else if (type == L"window"){
            return new FCWindow;
        }
        return 0;
    }
    
	void FCUIXml::createNative(){
		m_native = new FCNative;
	}
    
	void FCUIXml::createSubProperty(xmlNodePtr node, FCView *control){
		String strName = FCStr::stringTowstring((char*)node->name);
		String controlType = L"";
		if(control){
			controlType = control->getControlType();
		}
		if (strName == L"bands"){
			if (dynamic_cast<FCBandedGrid*>(control)){
				createBandedGridColumns(node, control);
			}
		}
		else if (strName == L"columns"){
            if (dynamic_cast<FCGrid*>(control)){
				createGridColumns(node, control);
            }
        }
		else if(strName == L"item" || strName == L"option"){
			if(dynamic_cast<FCComboBox*>(control)){
				FCComboBox *comboBox = dynamic_cast<FCComboBox*>(control);
				if(comboBox){
					createMenuItem(node, comboBox->getDropDownMenu(), 0);
				}
			}
			else if (dynamic_cast<FCMenu*>(control)){
                FCMenu *menu = dynamic_cast<FCMenu*>(control);
                if (menu){
                    createMenuItem(node, menu, 0);
                }
            }
		}
		else if(strName == L"nodes"){
			if(dynamic_cast<FCTree*>(control)){
				createTreeNodes(node, control);
			}
		}
		else if (strName == L"rows"){
            if (dynamic_cast<FCGrid*>(control)){
				createGridRows(node, control);
            }
        }
		else if(strName == L"tabpage"){
			if(dynamic_cast<FCTabControl*>(control)){
				createTabPage(node, control);
			}
		}
	    else if (strName == L"tr"){
            if (dynamic_cast<FCGrid*>(control)){
                FCGrid *grid = dynamic_cast<FCGrid*>(control);
                if ((int)grid->m_columns.size() == 0){
                    createGridColumns(node, control);
                }
                else{
                    createGridRow(node, control);
                }
            }
        }
	}
    
	FCView* FCUIXml::findControl(const String& name){
        for(int c = 0; c < m_controls.size(); c++){
			FCView *control = m_controls.get(c);
			if(control->getName() == name){
				return control;
			}
		}
		return 0;
	}
    
	HashMap<String, String> FCUIXml::getAttributes(xmlNodePtr node){
		HashMap<String, String> matrs;
        xmlAttr *atrNode = node->properties;
        while (atrNode){
            String strName = FCStr::stringTowstring((char*)atrNode->name);
            xmlChar *szAttr = xmlGetProp(node, atrNode->name);
            String strValue = FCStr::stringTowstring((char*)szAttr);
            matrs.put(strName, strValue);
            atrNode = atrNode->next;
        }
		return matrs;
	}
    
	ArrayList<FCView*> FCUIXml::getControls(){
		ArrayList<FCView*> controls;
		if(m_native){
			controls = m_native->m_controls;
		}
		return controls;
	}
    
	bool FCUIXml::isAfterSetingAttribute(const String& name)
    {
	    if (name == L"selectedindex"
            || name == L"selectedtext"
            || name == L"selectedvalue"
            || name == L"value"){
            return true;
        }
        else{
            return false;
        }
	}
    
	void FCUIXml::loadFile(const String& fileName, FCView *control){
		m_controls.clear();
		m_styles.clear();
        string xmlFileName = FCStr::wstringTostring(fileName);
        xmlDocPtr doc = xmlReadFile(xmlFileName.c_str(), 0, XML_PARSE_NOBLANKS);
        if(doc){
            xmlNodePtr node = xmlDocGetRootElement(doc);
            xmlNodePtr spChildNode = node->children;
            while (spChildNode){
                String nodeName = FCStr::stringTowstring((char*)spChildNode->name);
                if(nodeName == L"body"){
                    readBody(spChildNode, control);
                }
                else if(nodeName == L"head"){
                    readHead(spChildNode, control);
                }
                spChildNode = spChildNode->next;
            }
            xmlFreeDoc(doc);
        }
    }
    
	void FCUIXml::readBody(xmlNodePtr node, FCView *control)
	{
        xmlNodePtr spChildNode = node->children;
        while (spChildNode){
            readNode(spChildNode, control);
            spChildNode = spChildNode->next;
        }
	}
    
	void FCUIXml::onAddControl(FCView *control, xmlNodePtr node){
		m_controls.add(control);
		setEvents(node, control);
		m_event->callFunction(control, FCEventID::LOAD, m_event);
	}
    
	void FCUIXml::readChildNodes(xmlNodePtr node, FCView *control){
        xmlNodePtr spChildNode = node->children;
        while (spChildNode){
            readNode(spChildNode, control);
            spChildNode = spChildNode->next;
        }
	}
    
	void FCUIXml::readHead(xmlNodePtr node, FCView *control){
        xmlNodePtr spChildNode = node->children;
        while (spChildNode){
            String nodeName = FCStr::stringTowstring((char*)spChildNode->name);
			if(nodeName == L"script"){
				String nodeValue = L"";
                if(spChildNode->content){
                    nodeValue = FCStr::stringTowstring((char*)spChildNode->content);
                }
                else{
                    if(spChildNode->children){
                        if(spChildNode->children->content){
                            nodeValue = FCStr::stringTowstring((char*)spChildNode->children->content);
                        }
                    }
                }
				m_script->setText(nodeValue);
			}
			else if(nodeName == L"style"){
				readStyle(spChildNode, control);
			}
            spChildNode = spChildNode->next;
        }
	}
	
	FCView* FCUIXml::readNode(xmlNodePtr node, FCView* parent){
		String nodeName = FCStr::stringTowstring((char*)node->name);
		nodeName = FCStr::toLower(nodeName);
		FCView *control = createControl(node, nodeName);
		if(control){
			control->setNative(m_native);
            if (parent){
                parent->addControl(control);
            }
            else{
                m_native->addControl(control);
            }
			setAttributesBefore(node, control);
			if(dynamic_cast<FCSplitLayoutDiv*>(control)){
				createSplitLayoutSubProperty(node, dynamic_cast<FCSplitLayoutDiv*>(control));
			}
			else if (dynamic_cast<FCTableLayoutDiv*>(control)){
				createTableLayoutSubProperty(node, dynamic_cast<FCTableLayoutDiv*>(control));
			}
			else{
				readChildNodes(node, control);
			}
			setAttributesAfter(node, control);
			control->update();
			onAddControl(control, node);
		}
		else{
			createSubProperty(node, parent);
		}
		return control;
	}
	
	void FCUIXml::readStyle(xmlNodePtr node, FCView *control){
		String nodeValue = L"";
        if(node->content){
            nodeValue = FCStr::stringTowstring((char*)node->content);
        }
        else{
            if(node->children){
                if(node->children->content){
                    nodeValue = FCStr::stringTowstring((char*)node->children->content);
                }
            }
        }
		bool isstr = false;
		String str = L"";
		int len = (int)nodeValue.length();
		for(int i = 0; i < len; i++){
			wchar_t ch = nodeValue[i];
			if (ch == '\''){
				isstr = !isstr;
			}
			if (!isstr){
                if (ch == ';'){
					int idx = (int)str.find('{');
					String className = FCStr::toLower(str.substr(1, idx - 1));
                    String style = str.substr(idx + 1);
                    m_styles.put(className, style);
                }
                else if (ch == ' ' || ch ==  '\r' || ch == '\n' || ch == '\t'){
                    continue;
                }
            }
			wchar_t newCh[2] = {0};
			newCh[0] = ch;
			newCh[1] = '\0';
			str += newCh;
		}
	}
    
	void FCUIXml::setAttributesAfter(xmlNodePtr node, FCProperty *control)
	{
		HashMap<String, String> attributes = getAttributes(node);
        for(int a = 0; a < attributes.size(); a++){
			if(isAfterSetingAttribute(attributes.getKey(a))){
				control->setProperty(FCStr::toLower(attributes.getKey(a)), attributes.getValue(a));
			}
			else if (attributes.getKey(a) == L"class"){
				if(m_styles.containsKey(attributes.getKey(a))){
					setStyle(m_styles.get(attributes.getKey(a)), control);
				}
			}
			else if (attributes.getKey(a)== L"style"){
				setStyle(attributes.get(attributes.getKey(a)), control);
			}
		}
		attributes.clear();
	}
    
	void FCUIXml::setAttributesBefore(xmlNodePtr node, FCProperty *control){
		HashMap<String, String> attributes = getAttributes(node);
		for(int a = 0; a < attributes.size(); a++){
			if(!isAfterSetingAttribute(attributes.getKey(a))){
				control->setProperty(FCStr::toLower(attributes.getKey(a)), attributes.getValue(a));
			}
		}
		attributes.clear();
	}
    
	void FCUIXml::setEvents(xmlNodePtr node, FCProperty *control){
		FCView *baseControl = dynamic_cast<FCView*>(control);
		if(baseControl){
			HashMap<String, String> attributes = getAttributes(node);
			for(int a = 0; a < attributes.size(); a++){
				m_event->addEvent(baseControl, attributes.getKey(a), attributes.getValue(a));
			}
		}
	}
	
	void FCUIXml::setStyle(const String& style, FCProperty *control)
	{
		bool isstr = false;
		String str = L"";
		int len = (int)style.length();
		for(int i = 0; i < len; i++){
			wchar_t ch =  style[i];
			if (ch == '\''){
				isstr = !isstr;
			}
			if (!isstr){
                if (ch == ';'){
                    int idx = (int)str.find(L':');
                    String pName = str.substr(0, idx);
                    String pValue = str.substr(idx + 1);
					control->setProperty(FCStr::toLower(pName), pValue);
                    str = L"";
                    continue;
                }
                else if (ch == ' '){
                    continue;
                }
            }
			wchar_t newCh[2] = {0};
			newCh[0] = ch;
			newCh[1] = '\0';
			str += newCh;
		}
	}
}
