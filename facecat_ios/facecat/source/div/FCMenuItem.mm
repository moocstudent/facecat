#include "stdafx.h"
#include "FCMenuItem.h"

namespace FaceCat{
    FCMenuItem::FCMenuItem(){
        m_checked = false;
        m_dropDownMenu = 0;
        m_parentItem = 0;
        m_parentMenu = 0;
        FCFont font(L"宋体", 12, false, false, false);
        setFont(&font);
        FCSize size ={200, 25};
        setSize(size);
    }
    
    FCMenuItem::FCMenuItem(const String& text){
        m_checked = false;
        m_dropDownMenu = 0;
        m_parentItem = 0;
        m_parentMenu = 0;
        FCFont font(L"宋体", 12, false, false, false);
        setFont(&font);
        FCSize size ={200, 25};
        setSize(size);
        setText(text);
    }
    
    FCMenuItem::~FCMenuItem(){
        clearItems();
        m_dropDownMenu = 0;
        m_parentItem = 0;
        m_parentMenu = 0;
    }
    
    bool FCMenuItem::isChecked(){
        return m_checked;
    }
    
    void FCMenuItem::setChecked(bool checked){
        m_checked = checked;
    }
    
    FCMenu* FCMenuItem::getDropDownMenu(){
        return m_dropDownMenu;
    }
    
    void FCMenuItem::setDropDownMenu(FCMenu *dropDownMenu){
        m_dropDownMenu = dropDownMenu;
    }
    
    FCMenuItem* FCMenuItem::getParentItem(){
        return m_parentItem;
    }
    
    void FCMenuItem::setParentItem(FCMenuItem *parentItem){
        m_parentItem = parentItem;
    }
    
    FCMenu* FCMenuItem::getParentMenu(){
        return m_parentMenu;
    }
    
    void FCMenuItem::setParentMenu(FCMenu *parentMenu){
        m_parentMenu = parentMenu;
    }
    
    String FCMenuItem::getValue(){
        return m_value;
    }
    
    void FCMenuItem::setValue(const String& value){
        m_value = value;
    }
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void FCMenuItem::addItem(FCMenuItem *item){
        item->setParentItem(this);
        item->setParentMenu(m_parentMenu);
        item->onAddingItem(-1);
        m_items.add(item);
    }
    
    void FCMenuItem::clearItems(){
        ArrayList<FCMenuItem*> itemsCopy;
        int itemSize = (int)m_items.size();
        for (int i = 0; i < itemSize; i++){
            itemsCopy.add(m_items.get(i));
        }
        int copySize = (int)itemsCopy.size();
        for (int i = 0; i < copySize; i++){
            itemsCopy.get(i)->onRemovingItem();
            delete itemsCopy.get(i);
        }
        m_items.clear();
    }
    
    String FCMenuItem::getControlType(){
        return L"MenuItem";
    }
    
    ArrayList<FCMenuItem*> FCMenuItem::getItems(){
        return m_items;
    }
    
    void FCMenuItem::getProperty(const String& name, String *value, String *type){
        if(name == L"checked"){
            *type = L"bool";
            *value = FCStr::convertBoolToStr(isChecked());
        }
        else if(name == L"value"){
            *type = L"text";
            *value = getValue();
        }
        else{
            FCButton::getProperty(name, value, type);
        }
    }
    
    ArrayList<String> FCMenuItem::getPropertyNames(){
        ArrayList<String> propertyNames = FCButton::getPropertyNames();
        propertyNames.add(L"Checked");
        propertyNames.add(L"Value");
        return propertyNames;
    }
    
    void FCMenuItem::insertItem(int index, FCMenuItem *item){
        item->setParentItem(this);
        item->setParentMenu(m_parentMenu);
        item->onAddingItem(index);
        m_items.insert(index, item);
    }
    
    void FCMenuItem::onAddingItem(int index){
        FCMenu *dropDownMenu = 0;
        if (m_parentItem){
            dropDownMenu = m_parentItem->getDropDownMenu();
            if (!dropDownMenu){
                dropDownMenu = m_parentMenu->createDropDownMenu();
                dropDownMenu->setVisible(false);
                m_parentItem->setDropDownMenu(dropDownMenu);
                dropDownMenu->setParentItem(m_parentItem);
                m_parentMenu->getNative()->addControl(dropDownMenu);
            }
        }
        else{
            dropDownMenu = m_parentMenu;
        }
        if (dropDownMenu){
            if (index != -1){
                dropDownMenu->insertControl(index, this);
            }
            else{
                dropDownMenu->addControl(this);
            }
            int itemSize = (int)m_items.size();
            for (int i = 0; i < itemSize; i++){
                m_items.get(i)->onAddingItem(-1);
            }
        }
    }
    
    void FCMenuItem::onClick(FCTouchInfo touchInfo){
        FCButton::onClick(touchInfo);
        if (m_parentMenu){
            m_parentMenu->onMenuItemClick(this, touchInfo);
        }
    }
    
    void FCMenuItem::onTouchMove(FCTouchInfo touchInfo){
        FCButton::onTouchMove(touchInfo);
        if (m_parentMenu){
            m_parentMenu->onMenuItemTouchMove(this, touchInfo);
        }
    }
    
    void FCMenuItem::onPaintForeground(FCPaint *paint, const FCRect& clipRect){
        int width = getWidth(), height = getHeight();
        if(width > 0 && height > 0){
            int right = width;
            int midY = height / 2;
            String text = getText();
            int tRight = 0;
            Long textColor = getPaintingTextColor();
            if (text.length() > 0){
                FCFont *font = getFont();
                FCSize tSize = paint->textSize(text.c_str(), font);
                FCRect tRect ={0};
                tRect.left = 10;
                tRect.top = midY - tSize.cy / 2 + 2;
                tRect.right = tRect.left + tSize.cx;
                tRect.bottom = tRect.top + tSize.cy;
                paint->drawText(text.c_str(), textColor, font, tRect);
                tRight = tRect.right + 4;
            }
            if (m_checked){
                FCRect eRect ={tRight, height / 2 - 4, tRight + 8, height / 2 + 4};
                paint->fillEllipse(textColor, eRect);
            }
            if(m_items.size() > 0){
                FCPoint point1 ={0}, point2 ={0}, point3 ={0};
                FCMenu *menu = m_parentMenu;
                if (m_parentItem){
                    menu = m_parentItem->getDropDownMenu();
                }
                FCLayoutStyle layoutStyle = menu->getLayoutStyle();
                if (layoutStyle == FCLayoutStyle_LeftToRight || layoutStyle == FCLayoutStyle_RightToLeft){
                    point1.x = right - 25;
                    point1.y = midY - 2;
                    point2.x = right - 14;
                    point2.y = midY - 2;
                    point3.x = right - 20;
                    point3.y = midY + 4;
                }
                else{
                    point1.x = right - 15;
                    point1.y = midY;
                    point2.x = right - 25;
                    point2.y = midY - 5;
                    point3.x = right - 25;
                    point3.y = midY + 5;
                }
                FCPoint points[3];
                points[0] = point1;
                points[1] = point2;
                points[2] = point3;
                paint->fillPolygon(textColor, points, 3);
            }
        }
    }
    
    void FCMenuItem::onRemovingItem(){
        FCMenu *dropDownMenu = 0;
        if (m_parentItem){
            dropDownMenu = m_parentItem->getDropDownMenu();
        }
        else{
            dropDownMenu = m_parentMenu;
        }
        if (dropDownMenu){
            int itemSize = (int)m_items.size();
            for (int i = 0; i < itemSize; i++){
                m_items.get(i)->onRemovingItem();
            }
            dropDownMenu->removeControl(this);
        }
        if (m_dropDownMenu){
            m_parentMenu->getNative()->removeControl(m_dropDownMenu);
            delete m_dropDownMenu;
            m_dropDownMenu = 0;
        }
    }
    
    void FCMenuItem::removeItem(FCMenuItem *item){
        item->onRemovingItem();
        for(int i = 0; i < m_items.size(); i++){
            if(item == m_items.get(i)){
                m_items.removeAt(i);
                return;
            }
        }
    }
    
    void FCMenuItem::setProperty(const String& name, const String& value){
        if(name == L"checked"){
            setChecked(FCStr::convertStrToBool(value));
        }
        else if(name == L"value"){
            setValue(value);
        }
        else{
            FCButton::setProperty(name, value);
        }
    }
}
