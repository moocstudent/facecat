#include "..\\..\\stdafx.h"
#include "..\\..\\include\\xml\\FCUIXml.h"

namespace FaceCat{
	void FCUIXml::createBandedGridBands(IXMLDOMNode *node, FCView *control){
		FCBandedGrid *bandedGridA = dynamic_cast<FCBandedGrid*>(control);		
		HRESULT hr;
		long childCount = 0;
		IXMLDOMNodeList *spChildNodeList = 0;
		hr = node->get_childNodes(&spChildNodeList);
		hr = spChildNodeList->get_length(&childCount);
		for(long j = 0; j < childCount; j++){
			IXMLDOMNode *spChildNode = 0;
			hr = spChildNodeList->get_item(j, &spChildNode);
			BSTR name;
			hr = spChildNode->get_nodeName(&name);
			String subNodeName = name;
			if(name){
				SysFreeString(name);
			}
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
			if(spChildNode){
				spChildNode->Release();
			}
		}
		if(spChildNodeList){
			spChildNodeList->Release();
		}
	}
	
	void FCUIXml::createBandedGridColumns(IXMLDOMNode *node, FCView *control){
		FCGridBand *gridBand = dynamic_cast<FCGridBand*>(control);
		HRESULT hr;
		long childCount = 0;
		IXMLDOMNodeList *spChildNodeList = 0;
		hr = node->get_childNodes(&spChildNodeList);
		hr = spChildNodeList->get_length(&childCount);
		for(long j = 0; j < childCount; j++){
			IXMLDOMNode *spChildNode = 0;
			hr = spChildNodeList->get_item(j, &spChildNode);
			BSTR name;
			hr = spChildNode->get_nodeName(&name);
			String subNodeName = name;
			if(name){
				SysFreeString(name);
			}
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
			if(spChildNode){
				spChildNode->Release();
			}
		}
		if(spChildNodeList){
			spChildNodeList->Release();
		}
	}

	void FCUIXml::createChartSubProperty(IXMLDOMNode *node, FCChart *chart){
		HRESULT hr;
		long childCount = 0;
		IXMLDOMNodeList *spChildNodeList = 0;
		hr = node->get_childNodes(&spChildNodeList);
		hr = spChildNodeList->get_length(&childCount);
		for(long j = 0; j < childCount; j++){
			IXMLDOMNode *spChildNode = 0;
			hr = spChildNodeList->get_item(j, &spChildNode);
			BSTR name;
			hr = spChildNode->get_nodeName(&name);
			String subNodeName = name;
			if(name){
				SysFreeString(name);
			}
			if (subNodeName == L"div") {
				ChartDiv *div = chart->addDiv();
				setAttributesBefore(spChildNode, div);
				IXMLDOMNodeList *spChildNodeList2 = 0;
				hr = spChildNode->get_childNodes(&spChildNodeList2);
				long childCount2 = 0;
				hr = spChildNodeList2->get_length(&childCount2);
				for(long s = 0; s < childCount2; s++){
					IXMLDOMNode *spChildNode2 = 0;
					hr = spChildNodeList2->get_item(2, &spChildNode2);
					BSTR name2;
					hr = spChildNode2->get_nodeName(&name2);
					String subNodeName2 = name2;
					if(name2){
						SysFreeString(name2);
					}
					if(subNodeName2 == L"titlebar"){
						ChartTitleBar *titleBar = div->getTitleBar();
						setAttributesBefore(spChildNode2, titleBar);
						setAttributesAfter(spChildNode2, titleBar);
					}
					if(spChildNode2){
						spChildNode2->Release();
					}
				}
				if(spChildNodeList2){
					spChildNodeList2->Release();
				}
				setAttributesAfter(spChildNode, div);
			}
			if(spChildNode){
				spChildNode->Release();
			}
		}
		if(spChildNodeList){
			spChildNodeList->Release();
		}
	}
	
	void FCUIXml::createMenuItem(IXMLDOMNode *node, FCMenu *menu, FCMenuItem *parentItem){
		HRESULT hr;
		BSTR name;
		hr = node->get_nodeName(&name);
		String nodeName = name;
		if(name){
			SysFreeString(name);
		}
		FCMenuItem *item = new FCMenuItem;
		long childCount = 0;
		item->setNative(m_native);
		setAttributesBefore(node, item);
		if (parentItem){
			parentItem->addItem(item);
		}
		else{
			menu->addItem(item);
		}
		IXMLDOMNodeList *spChildNodeList = 0;
		hr = node->get_childNodes(&spChildNodeList);
		hr = spChildNodeList->get_length(&childCount);
		int col = 0;
		for(long j = 0; j < childCount; j++){
			IXMLDOMNode *spChildNode = 0;
			hr = spChildNodeList->get_item(j, &spChildNode);
			createMenuItem(spChildNode, menu, item);
			if(spChildNode){
				spChildNode->Release();
			}
		}
		if(spChildNodeList){
			spChildNodeList->Release();
		}
		setAttributesAfter(node, item);
		onAddControl(item, node);
	}

	void FCUIXml::createGridColumns(IXMLDOMNode *node, FCView *control){
		FCGrid *grid = dynamic_cast<FCGrid*>(control);
		HRESULT hr;
		long childCount = 0;
		IXMLDOMNodeList *spChildNodeList = 0;
		hr = node->get_childNodes(&spChildNodeList);
		hr = spChildNodeList->get_length(&childCount);
		for(long j = 0; j < childCount; j++){
			IXMLDOMNode *spChildNode = 0;
			hr = spChildNodeList->get_item(j, &spChildNode);
			BSTR name;
			hr = spChildNode->get_nodeName(&name);
			String subNodeName = name;
			if(name){
				SysFreeString(name);
			}
			FCView *subControl = createControl(spChildNode, FCStr::toLower(subNodeName));
			FCGridColumn *column = dynamic_cast<FCGridColumn*>(subControl);
			if(column){
				column->setNative(m_native);
				grid->addColumn(column);
			}
			setAttributesBefore(spChildNode, column);
			readChildNodes(spChildNode, column);
			setAttributesAfter(spChildNode, column);
			BSTR value;
			hr = spChildNode->get_text(&value);
			String subNodeValue = value;
			if(value){
				SysFreeString(value);
			}
		    if((int)subNodeValue.length() > 0){
                column->setText(subNodeValue);
            }
			onAddControl(column, spChildNode);
			if(spChildNode){
				spChildNode->Release();
			}
		}
		if(spChildNodeList){
			spChildNodeList->Release();
		}
	}

	void FCUIXml::createGridRow(IXMLDOMNode *node, FCView *control){
		HRESULT hr;
		FCGrid *grid = dynamic_cast<FCGrid*>(control);
		FCGridRow *row = new FCGridRow;
		grid->addRow(row);
		setAttributesBefore(node, row);
		IXMLDOMNodeList *spChildNodeList2 = 0;
		hr = node->get_childNodes(&spChildNodeList2);
		long childCount2 = 0;
		hr = spChildNodeList2->get_length(&childCount2);
		int col = 0;
		for(long j = 0; j < childCount2; j++){
			IXMLDOMNode *spChildNode2 = 0;
			hr = spChildNodeList2->get_item(j, &spChildNode2);
			BSTR name2;
			hr = spChildNode2->get_nodeName(&name2);
			String subNodeName = name2;
			if(name2){
				SysFreeString(name2);
			}
			BSTR value2;
			hr = spChildNode2->get_text(&value2);
			String subNodeValue = value2;
			if(value2){
				SysFreeString(value2);
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
				else if (cellType == L"combobox"){
					cell = new FCGridComboBoxCell;
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
			spChildNode2->Release();
		}
		spChildNodeList2->Release();
		setAttributesAfter(node, row);
	}

	void FCUIXml::createGridRows(IXMLDOMNode *node, FCView *control){
		HRESULT hr;
		long childCount = 0;
		IXMLDOMNodeList *spChildNodeList = 0;
		hr = node->get_childNodes(&spChildNodeList);
		hr = spChildNodeList->get_length(&childCount);
		for(long j = 0; j < childCount; j++){
			IXMLDOMNode *spChildNode = 0;
			hr = spChildNodeList->get_item(j, &spChildNode);
			BSTR name;
			hr = spChildNode->get_nodeName(&name);
			String nodeName = name;
			if(name){
				SysFreeString(name);
			}
			if(nodeName == L"row" || nodeName == L"tr"){
				createGridRow(spChildNode, control);
			}
			if(spChildNode){
				spChildNode->Release();
			}
		}
		if(spChildNodeList){
			spChildNodeList->Release();
		}
	}

	void FCUIXml::createSplitLayoutSubProperty(IXMLDOMNode *node, FCSplitLayoutDiv *splitLayoutDiv){
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

	void FCUIXml::createTableLayoutSubProperty(IXMLDOMNode *node, FCTableLayoutDiv *tableLayoutDiv ){
		setAttributesBefore(node, tableLayoutDiv);
		long countBig = 0;
		IXMLDOMNodeList *spChildNodeBigs = 0;
		HRESULT hrBig = node->get_childNodes(&spChildNodeBigs);
		hrBig = spChildNodeBigs->get_length(&countBig);
		for(long i = 0; i < countBig; i++){
			IXMLDOMNode *nodeBig = 0;
			hrBig = spChildNodeBigs->get_item(i, &nodeBig);
			BSTR name;
			hrBig = nodeBig->get_nodeName(&name);
			String nodeName = name;
			if(name){
				SysFreeString(name);
			}
			if(nodeName == L"columnstyles"){
				long countColumn = 0;
				IXMLDOMNodeList *spChildNodeColumns = 0;
				HRESULT hrColumn = nodeBig->get_childNodes(&spChildNodeColumns);
				hrColumn = spChildNodeColumns->get_length(&countColumn);
				for (int m = 0; m < countColumn; m++){
					IXMLDOMNode *nodeColumn = 0;
					hrColumn = spChildNodeColumns->get_item(m, &nodeColumn);
					BSTR tempName;
					nodeColumn->get_nodeName(&tempName);
					String str = tempName;
					if (str == L"columnstyle"){
						FCColumnStyle column(FCSizeType_PercentSize, 0.0f);
						HashMap<String, String> mp = getAttributes(nodeColumn);
						for(int a = 0; a < mp.size(); a++){
							column.setProperty(mp.getKey(a), mp.getValue(a));
						}
						tableLayoutDiv->m_columnStyles.add(column);

					}
				}
			}
			else if(nodeName == L"rowstyles"){
				long countRow = 0;
				IXMLDOMNodeList *spChildNodeRows = 0;
				HRESULT hrRow = nodeBig->get_childNodes(&spChildNodeRows);
				hrRow = spChildNodeRows->get_length(&countRow);
				for (int m = 0; m < countRow; m++){
					IXMLDOMNode *nodeRow = 0;
					spChildNodeRows->get_item(m, &nodeRow);
					BSTR tempName;
					nodeRow->get_nodeName(&tempName);
					String str = tempName;
					if (str == L"rowstyle"){
						FCRowStyle row(FCSizeType_PercentSize, 0.0f);
						HashMap<String, String> mp = getAttributes(nodeRow);
						for(int a = 0; a < mp.size(); a++){
							row.setProperty(mp.getKey(a), mp.getValue(a));
						}
						tableLayoutDiv->m_rowStyles.add(row);
					}
				}
			}
			else if (nodeName == L"childs"){
				readChildNodes(nodeBig, tableLayoutDiv);                   
			}
		}
		setAttributesAfter(node, tableLayoutDiv);
		tableLayoutDiv->update();
		onAddControl(tableLayoutDiv, node);
	}

	void FCUIXml::createTabPage(IXMLDOMNode *node, FCView *control)
	{
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

	void FCUIXml::createTreeNode(IXMLDOMNode *node, FCView *control, FCTreeNode *treeNode){
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
			HRESULT hr;
			long childCount = 0;
			IXMLDOMNodeList *spChildNodeList = 0;
			hr = node->get_childNodes(&spChildNodeList);
			hr = spChildNodeList->get_length(&childCount);
			for(long j = 0; j < childCount; j++){
				IXMLDOMNode *spChildNode = 0;
				hr = spChildNodeList->get_item(j, &spChildNode);
				BSTR name;
				hr = spChildNode->get_nodeName(&name);
				String nodeName = name;
				if(name){
					SysFreeString(name);
				}
				if(nodeName == L"node"){
					createTreeNode(spChildNode, control, appendNode);
				}
				if(spChildNode){
					spChildNode->Release();
				}
			}
			if(spChildNodeList){
				spChildNodeList->Release();
			}
			setAttributesAfter(node, appendNode);
        }
	}

	void FCUIXml::createTreeNodes(IXMLDOMNode *node, FCView *control){
		HRESULT hr;
		long childCount = 0;
		IXMLDOMNodeList *spChildNodeList = 0;
		hr = node->get_childNodes(&spChildNodeList);
		hr = spChildNodeList->get_length(&childCount);
		for(long j = 0; j < childCount; j++){
			IXMLDOMNode *spChildNode = 0;
			hr = spChildNodeList->get_item(j, &spChildNode);
			createTreeNode(spChildNode, control, 0);
			if(spChildNode){
				spChildNode->Release();
			}
		}
		if(spChildNodeList){
			spChildNodeList->Release();
		}
	}

	FCView* FCUIXml::createUserControl(IXMLDOMNode *node){
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

	void FCUIXml::dprintf( char * format, ...){
		static char buf[1024];
		va_list args;
		va_start( args, format );
		vsprintf_s( buf, format, args );
		va_end( args);
		OutputDebugStringA( buf);
		printf("%s", buf);
	}

	IXMLDOMDocument* FCUIXml::domFromCOM(){
	   HRESULT hr;
	   IXMLDOMDocument *pxmldoc = 0;
	 
	   HRCALL( CoCreateInstance(__uuidof(MSXML2::DOMDocument30),
					  NULL,
					  CLSCTX_INPROC_SERVER,
					  __uuidof(IXMLDOMDocument),
					  (void**)&pxmldoc),
					  "Create a new DOMDocument");
	 
		HRCALL( pxmldoc->put_async(VARIANT_FALSE),
				"should never fail");
		HRCALL( pxmldoc->put_validateOnParse(VARIANT_FALSE),
				"should never fail");
		HRCALL( pxmldoc->put_resolveExternals(VARIANT_FALSE),
				"should never fail");
	 
		return pxmldoc;
	clean:
		if (pxmldoc){
			pxmldoc->Release();
		}
		return NULL;
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

	FCChart* FCUIXml::getChart(const String& name){
		FCView *control = findControl(name);
        if (control){
            return dynamic_cast<FCChart*>(control);
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

	FCDateTimePicker* FCUIXml::getDateTimePicker(const String& name){
		FCView *control = findControl(name);
        if (control){
            return dynamic_cast<FCDateTimePicker*>(control);
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

	ArrayList<FCView*> FCUIXml::getLikeControls(const String& name){
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

	FCView* FCUIXml::createControl(IXMLDOMNode *node, const String& type){
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

	void FCUIXml::createSubProperty(IXMLDOMNode *node, FCView *control){
		BSTR name;
		HRESULT hr;
		hr = node->get_nodeName(&name);
		String strName = name;
		if(name){
			SysFreeString(name);
		}
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

	HashMap<String, String> FCUIXml::getAttributes(IXMLDOMNode *node){
		HashMap<String, String> matrs;
		IXMLDOMAttribute *attribute = 0;
		IXMLDOMNamedNodeMap *attributes = 0;
		HRESULT hr;
		hr = node->get_attributes(&attributes);
		long mCount = 0;
		hr = attributes->get_length(&mCount);
		for(long j = 0; j < mCount; ++j){
			IXMLDOMNode *atrNode = 0;
			hr = attributes->get_item(j, &atrNode);
			BSTR name;
			hr = atrNode->get_nodeName(&name);
			String strName = name;
			if(name){
				SysFreeString(name);
			}
			BSTR value;
			hr = atrNode->get_text(&value);
			String strValue = value;
			if(value){
				SysFreeString(value);
			}
			matrs.put(strName, strValue);
			if(atrNode){
				atrNode->Release();
			}
		}
		if(attributes){
			attributes->Release();
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

	void FCUIXml::loadFile(const String& fileName, FCView *control)
	{
		m_controls.clear();
		m_styles.clear();
		IXMLDOMDocument *pXMLDom = 0;
		IXMLDOMParseError *pXMLErr = 0;
		IXMLDOMElement *spElement = 0;
		IXMLDOMNodeList *spNodeList = 0;
		BSTR bstr = 0;
		VARIANT_BOOL status;
		VARIANT var;
		HRESULT hr;
		long lCount = 0;
		pXMLDom = domFromCOM();
		if (!pXMLDom) goto clean;
		VariantInit(&var);
		V_BSTR(&var) = SysAllocString(fileName.c_str());
		V_VT(&var) = VT_BSTR;
		HRCALL(pXMLDom->load(var, &status), "");
		if (status != VARIANT_TRUE) {
			HRCALL(pXMLDom->get_parseError(&pXMLErr), "");
			HRCALL(pXMLErr->get_reason(&bstr), "");
			goto clean;
		}
		HRCALL(pXMLDom->get_xml(&bstr), "");
		hr = pXMLDom->get_documentElement(&spElement);
		hr = spElement->get_childNodes(&spNodeList); 
		hr = spNodeList->get_length(&lCount);
		for (long i = 0; i < lCount; ++i){
			IXMLDOMNode *spChildNode = 0;
			hr = spNodeList->get_item(i, &spChildNode);
			BSTR name;
			hr = spChildNode->get_nodeName(&name);
			String nodeName = name;
			if(name){
				SysFreeString(name);
			}
			if(nodeName == L"body"){
				readBody(spChildNode, control);
			}
			else if(nodeName == L"head"){
				readHead(spChildNode, control);
			}
			if(spChildNode){
				spChildNode->Release();
			}
		}
		clean:
		if (bstr) SysFreeString(bstr);
		if (&var) VariantClear(&var);
		if (pXMLErr) pXMLErr->Release();
		if (pXMLDom) pXMLDom->Release();
		if (spElement) spElement->Release();
		if (spNodeList) spNodeList->Release();
	}

	void FCUIXml::readBody(IXMLDOMNode *node, FCView *control){
		HRESULT hr;
		long childCount = 0;
		IXMLDOMNodeList *spChildNodeList = 0;
		hr = node->get_childNodes(&spChildNodeList);
		hr = spChildNodeList->get_length(&childCount);
		for(long j = 0; j < childCount; j++){
			IXMLDOMNode *spChildNode = 0;
			hr = spChildNodeList->get_item(j, &spChildNode);
			readNode(spChildNode, control);
			if(spChildNode){
				spChildNode->Release();
			}
		}
		if(spChildNodeList){
			spChildNodeList->Release();
		}		
	}

	void FCUIXml::onAddControl(FCView *control, IXMLDOMNode *node)
	{
		m_controls.add(control);
		setEvents(node, control);
		m_event->callFunction(control, FCEventID::LOAD, m_event);
	}

	void FCUIXml::readChildNodes(IXMLDOMNode *node, FCView *control)
	{
		HRESULT hr;
		long childCount = 0;
		IXMLDOMNodeList *spChildNodeList = 0;
		hr = node->get_childNodes(&spChildNodeList);
		hr = spChildNodeList->get_length(&childCount);
		for(long j = 0; j < childCount; j++){
			IXMLDOMNode *spChildNode = 0;
			hr = spChildNodeList->get_item(j, &spChildNode);
			readNode(spChildNode, control);
			if(spChildNode){
				spChildNode->Release();
			}
		}
		if(spChildNodeList){
			spChildNodeList->Release();
		}
	}

	void FCUIXml::readHead(IXMLDOMNode *node, FCView *control)
	{
		HRESULT hr;
		long childCount = 0;
		IXMLDOMNodeList *spChildNodeList = 0;
		hr = node->get_childNodes(&spChildNodeList);
		hr = spChildNodeList->get_length(&childCount);
		for(long j = 0; j < childCount; j++){
			IXMLDOMNode *spChildNode = 0;
			hr = spChildNodeList->get_item(j, &spChildNode);
			BSTR name;
			hr = spChildNode->get_nodeName(&name);
			String nodeName = name;
			if(name){
				SysFreeString(name);
			}
			if(nodeName == L"script"){
				BSTR value;
				hr = spChildNode->get_text(&value);
				String nodeValue = value;
				m_script->setText(nodeValue);
				if(value){
					SysFreeString(value);
				}
			}			
			else if(nodeName == L"style"){
				readStyle(spChildNode, control);
			}
			if(spChildNode){
				spChildNode->Release();
			}
		}
		if(spChildNodeList){
			spChildNodeList->Release();
		}		
	}
	
	FCView* FCUIXml::readNode(IXMLDOMNode *node, FCView* parent){
		HRESULT hr;
		BSTR name;
		hr = node->get_nodeName(&name);
		String nodeName = name;
		if(name){
			SysFreeString(name);
		}
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
			else if(dynamic_cast<FCChart*>(control)){
				createChartSubProperty(node, dynamic_cast<FCChart*>(control));
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

	void FCUIXml::readStyle(IXMLDOMNode *node, FCView *control)
	{
		HRESULT hr;
		BSTR value;
		hr = node->get_text(&value);
		String nodeValue = value;
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
		if(value){
			SysFreeString(value);
		}
	}

	void FCUIXml::setAttributesAfter(IXMLDOMNode *node, FCProperty *control)
	{
		HashMap<String, String> attributes = getAttributes(node);
		for(int a = 0; a < attributes.size(); a++){
			if(isAfterSetingAttribute(attributes.getKey(a))){
				control->setProperty(FCStr::toLower(attributes.getKey(a)), attributes.getValue(a));
			}
			else if (attributes.getKey(a) == L"class"){
				if(m_styles.containsKey(attributes.get(attributes.getKey(a))))
				{
					setStyle(m_styles.get(attributes.get(attributes.getKey(a))), control);
				}
			}
			else if (attributes.getKey(a) == L"style"){
				setStyle(attributes.get(attributes.getKey(a)), control);
			}
		}
		attributes.clear();
	}

	void FCUIXml::setAttributesBefore(IXMLDOMNode *node, FCProperty *control){
		HashMap<String, String> attributes = getAttributes(node);
		for(int a = 0; a < attributes.size(); a++){
			if(!isAfterSetingAttribute(attributes.getKey(a))){
				control->setProperty(FCStr::toLower(attributes.getKey(a)), attributes.getValue(a));
			}
		}
		attributes.clear();
	}

	void FCUIXml::setEvents(IXMLDOMNode *node, FCProperty *control){
		FCView *baseControl = dynamic_cast<FCView*>(control);
		if(baseControl){
			HashMap<String, String> attributes = getAttributes(node);
			for(int a = 0; a < attributes.size(); a++){
				m_event->addEvent(baseControl, attributes.getKey(a), attributes.getValue(a));
			}
		}
	}

	void FCUIXml::setStyle(const String& style, FCProperty *control){
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