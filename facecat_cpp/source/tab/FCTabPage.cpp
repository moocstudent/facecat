#include "..\\..\\stdafx.h"
#include "..\\..\\include\\tab\\FCTabPage.h"

namespace FaceCat
{
	void FCTabPage::dragHeaderBegin(Object sender, Object pInvoke)
	{
		FCTabPage *tabPage = (FCTabPage*)pInvoke;
		if(tabPage)
		{
			tabPage->getTabControl()->onDragTabHeaderBegin(tabPage);
		}
	}

	void FCTabPage::dragHeaderEnd(Object sender, Object pInvoke)
	{
		FCTabPage *tabPage = (FCTabPage*)pInvoke;
		if(tabPage)
		{
			tabPage->getTabControl()->onDragTabHeaderEnd(tabPage);
		}
	}

	void FCTabPage::draggingHeader(Object sender, Object pInvoke)
	{
		FCTabPage *tabPage = (FCTabPage*)pInvoke;
		if(tabPage)
		{
			tabPage->getTabControl()->onDraggingTabHeader(tabPage);
		}
	}

	void FCTabPage::headerTouchDown(Object sender, FCTouchInfo touchInfo, Object pInvoke)
	{
		FCTabPage *tabPage = (FCTabPage*)pInvoke;
		if(tabPage)
		{
			tabPage->getTabControl()->setSelectedTabPage(tabPage);
			tabPage->getTabControl()->invalidate();
		}
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCTabPage::FCTabPage()
	{
		m_dragHeaderBeginEvent = dragHeaderBegin;
        m_dragHeaderEndEvent = dragHeaderEnd;
        m_draggingHeaderEvent = draggingHeader;
		m_headerTouchDownEvent = headerTouchDown;
		m_headerButton = 0;
		m_headerLocation.x = 0;
		m_headerLocation.y = 0;
		m_tabControl = 0;
	}

	FCTabPage::~FCTabPage()
	{
		if (m_dragHeaderBeginEvent)
        {
            if (m_headerButton)
            {
				m_headerButton->removeEvent(m_dragHeaderBeginEvent, FCEventID::DRAGBEGIN);
            }
            m_dragHeaderBeginEvent = 0;
        }
        if (m_dragHeaderEndEvent)
        {
            if (m_headerButton)
            {
                m_headerButton->removeEvent(m_dragHeaderEndEvent, FCEventID::DRAGEND);
            }
            m_dragHeaderEndEvent = 0;
        }
        if (m_draggingHeaderEvent)
        {
            if (m_headerButton)
            {
                m_headerButton->removeEvent(m_draggingHeaderEvent, FCEventID::DRAGGING);
            }
            m_draggingHeaderEvent = 0;
        }
	    if (m_headerTouchDownEvent)
        {
            if (m_headerButton)
            {
                m_headerButton->removeEvent(m_headerTouchDownEvent, FCEventID::TOUCHDOWN);
            }
            m_headerTouchDownEvent = 0;
        }
		m_headerButton = 0;
		m_tabControl = 0;
	}

	FCButton* FCTabPage::getHeaderButton()
	{
		return m_headerButton;
	}

	void FCTabPage::setHeaderButton(FCButton *headerButton)
	{
		m_headerButton = headerButton;
	}

	FCPoint FCTabPage::getHeaderLocation()
	{
		return m_headerLocation;
	}

	void FCTabPage::setHeaderLocation(FCPoint headerLocation)
	{
		m_headerLocation = headerLocation;
	}

	bool FCTabPage::isHeaderVisible()
	{
		if(m_headerButton)
		{
			return m_headerButton->isVisible();
		}
		else
		{
			return false;
		}
	}

	void FCTabPage::setHeaderVisible(bool visible)
	{
		if(m_headerButton)
		{
			m_headerButton->setVisible(visible);
		}
	}

	FCTabControl* FCTabPage::getTabControl()
	{
		return m_tabControl;
	}

	void FCTabPage::setTabControl(FCTabControl *tabControl)
	{
		m_tabControl = tabControl;
	}

	////////////////////////////////////////////////////////////////////////////////////////////

	String FCTabPage::getControlType()
	{
		return L"TabPage";
	}

	void FCTabPage::getProperty(const String& name, String *value, String *type)
	{
		if(name == L"headersize")
		{
			*type = L"size";
			if(m_headerButton)
			{
				*value = FCStr::convertSizeToStr(m_headerButton->getSize());
			}
			else
			{
				*value = L"0,0";
			}
		}
		else if(name == L"headervisible")
		{
			*type = L"bool";
			*value = FCStr::convertBoolToStr(isHeaderVisible());
		}
	    else if ((int)name.find(L"header-") != -1)
        {
            if (m_headerButton)
            {
                m_headerButton->getProperty(name.substr(7), value, type);
            }
        }

		else
		{
			FCDiv::getProperty(name, value, type);
		}
	}

	ArrayList<String> FCTabPage::getPropertyNames()
	{
		ArrayList<String> propertyNames = FCDiv::getPropertyNames();
		propertyNames.add(L"Header");
		propertyNames.add(L"HeaderSize");
		propertyNames.add(L"HeaderVisible");
		return propertyNames;
	}

	void FCTabPage::onLoad()
	{
		FCDiv::onLoad();
	if(m_tabControl)
	{
        if (!m_headerButton)
        {
			FCHost *host = getNative()->getHost();
            m_headerButton = dynamic_cast<FCButton*>(host->createInternalControl(this, L"headerbutton"));
			m_headerButton->addEvent(m_dragHeaderBeginEvent, FCEventID::DRAGBEGIN, this);
            m_headerButton->addEvent(m_dragHeaderEndEvent, FCEventID::DRAGEND, this);
            m_headerButton->addEvent(m_draggingHeaderEvent, FCEventID::DRAGGING, this);
			m_headerButton->addEvent(m_headerTouchDownEvent, FCEventID::TOUCHDOWN, this);
        }
			if(!m_tabControl->containsControl(m_headerButton))
			{
				m_tabControl->addControl(m_headerButton);
			}
	}
	}

	void FCTabPage::onTextChanged()
	{
		FCDiv::onTextChanged();
		if(m_headerButton)
		{
			m_headerButton->setText(getText());
		}
	}

	void FCTabPage::setProperty(const String& name, const String& value)
	{
		if(name == L"headersize")
		{
			if(m_headerButton)
			{
				m_headerButton->setProperty(L"size", value);
			}
		}
		else if(name == L"headervisible")
		{
			setHeaderVisible(FCStr::convertStrToBool(value));
		}
	    else if ((int)name.find(L"header-") != -1)
        {
            if (m_headerButton)
            {
                m_headerButton->setProperty(name.substr(7), value);
            }
        }
		else
		{
			FCDiv::setProperty(name, value);
		}
	}
}