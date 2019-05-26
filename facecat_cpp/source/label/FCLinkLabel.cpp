#include "..\\..\\stdafx.h"
#include "..\\..\\include\\Label\\FCLinkLabel.h"

namespace FaceCat{
	Long FCLinkLabel::getPaintingLinkColor(){
		if (isEnabled()){
			FCNative *native = getNative();
			if(this == native->getHoveredControl()){
				return m_activeLinkColor;
			}
			else if(this == native->getPushedControl()){
				return m_activeLinkColor;
			}
			else{
				if (m_linkVisited && m_visited){
					return m_visitedLinkColor;
				}
				else{
					return m_linkColor;
				}
			}
		}
		else{
			return m_disabledLinkColor;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	FCLinkLabel::FCLinkLabel(){
		m_activeLinkColor = FCColor::argb(255, 0, 0);
		m_disabledLinkColor = FCColor::argb(133, 133, 133);
		m_linkBehavior = LinkBehaviorA_AlwaysUnderLine;
		m_linkColor = FCColor::argb(0, 0, 255);
		m_linkVisited = false;
		m_visited = false;
		m_visitedLinkColor = FCColor::argb(128, 0, 128);
		setCursor(FCCursors_Hand);
	}

	FCLinkLabel::~FCLinkLabel(){
	}

	Long FCLinkLabel::getActiveLinkColor(){
		return m_activeLinkColor;
	}

	void FCLinkLabel::setActiveLinkColor(Long activeLinkColor){
		m_activeLinkColor = activeLinkColor;
	}

	Long FCLinkLabel::getDisabledLinkColor(){
		return m_disabledLinkColor;
	}

	void FCLinkLabel::setDisabledLinkColor(Long disabledLinkColor){
		m_disabledLinkColor = disabledLinkColor;
	}

	FCLinkBehavior FCLinkLabel::getLinkBehavior(){
		return m_linkBehavior;
	}

	void FCLinkLabel::setLinkBehavior(FCLinkBehavior linkBehavior){
		m_linkBehavior = linkBehavior;
	}

	Long FCLinkLabel::getLinkColor(){
		return m_linkColor;
	}

	void FCLinkLabel::setLinkColor(Long linkColor){
		m_linkColor = linkColor;
	}

	bool FCLinkLabel::isLinkVisited(){
		return m_linkVisited;
	}

	void FCLinkLabel::setLinkVisited(bool linkVisited){
		m_linkVisited = linkVisited;
	}

	Long FCLinkLabel::getVisitedLinkColor(){
		return m_visitedLinkColor;
	}

	void FCLinkLabel::setVisitedLinkColor(Long visitedLinkColor){
		m_visitedLinkColor = visitedLinkColor;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	String FCLinkLabel::getControlType(){
		return L"LinkLabel";
	}

	void FCLinkLabel::getProperty(const String& name, String *value, String *type){
		if (name == L"activelinkcolor"){
			*type = L"color";
			*value = FCStr::convertColorToStr(getActiveLinkColor());
		}
		else if (name == L"disabledlinkcolor"){
			*type = L"color";
			*value = FCStr::convertColorToStr(getDisabledLinkColor());
		}
		else if (name == L"linkbehavior"){
			*type = L"enum:FCLinkBehavior";
			FCLinkBehavior linkBehavior = getLinkBehavior();
			if (linkBehavior == LinkBehaviorA_AlwaysUnderLine){
				*value = L"AlwaysUnderLine";
			}
			else if (linkBehavior == LinkBehaviorA_HoverUnderLine){
				*value = L"HoverUnderLine";
			}
			else{
				*value = L"NeverUnderLine";
			}
		}
		else if (name == L"linkcolor"){
			*type = L"color";
			*value = FCStr::convertColorToStr(getLinkColor());
		}
		else if (name == L"linkvisited"){
			*type = L"bool";
			*value = FCStr::convertBoolToStr(isLinkVisited());
		}
		else if (name == L"visitedlinkcolor"){
			*type = L"color";
			*value = FCStr::convertColorToStr(getVisitedLinkColor());
		}
		else{
			FCLabel::getProperty(name, value, type);
		}
	}

	ArrayList<String> FCLinkLabel::getPropertyNames(){
		ArrayList<String> propertyNames = FCLabel::getPropertyNames();
		propertyNames.add(L"ActiveLinkColor");
		propertyNames.add(L"DisabledLinkColor");
		propertyNames.add(L"LinkBehavior");
		propertyNames.add(L"LinkColor");
		propertyNames.add(L"LinkVisited");
		propertyNames.add(L"VisitedLinkColor");
		return propertyNames;
	}

	void FCLinkLabel::onClick(FCTouchInfo touchInfo){
		FCLabel::onClick(touchInfo);
		if (m_linkVisited){
			m_visited = true;
		}
	}

	void FCLinkLabel::onTouchDown(FCTouchInfo touchInfo){
		FCLabel::onTouchDown(touchInfo);
		invalidate();
	}

	void FCLinkLabel::onTouchEnter(FCTouchInfo touchInfo){
		FCLabel::onTouchEnter(touchInfo);
		invalidate();
	}

	void FCLinkLabel::onTouchLeave(FCTouchInfo touchInfo){
		FCLabel::onTouchLeave(touchInfo);
		invalidate();
	}

	void FCLinkLabel::onTouchUp(FCTouchInfo touchInfo){
		FCLabel::onTouchUp(touchInfo);
		invalidate();
	}

	void FCLinkLabel::onPaintForeground(FCPaint *paint, const FCRect& clipRect){
		String text = getText();
		if (text.length() > 0){
			int width = getWidth(), height = getHeight();
			if(width > 0 && height > 0){
				FCFont *font = getFont();
				FCSize tSize = paint->textSize(text.c_str(), font);
				Long linkColor = getPaintingLinkColor();
				FCPoint tPoint = {(width - tSize.cx) / 2, (height - tSize.cy) / 2};
				FCPadding padding = getPadding();
                switch (m_textAlign){
                    case FCContentAlignment_BottomCenter:
                        tPoint.y = height - tSize.cy;
                        break;
                    case FCContentAlignment_BottomLeft:
                        tPoint.x = padding.left;
                        tPoint.y = height - tSize.cy - padding.bottom;
                        break;
                    case FCContentAlignment_BottomRight:
                        tPoint.x = width - tSize.cx - padding.right;
                        tPoint.y = height - tSize.cy - padding.bottom;
                        break;
                    case FCContentAlignment_MiddleLeft:
                        tPoint.x = padding.left;
                        break;
                    case FCContentAlignment_MiddleRight:
                        tPoint.x = width - tSize.cx - padding.right;
                        break;
                    case FCContentAlignment_TopCenter:
                        tPoint.y = padding.top;
                        break;
                    case FCContentAlignment_TopLeft:
                        tPoint.x = padding.left;
                        tPoint.y = padding.top;
                        break;
                    case FCContentAlignment_TopRight:
                        tPoint.x = width - tSize.cx - padding.right;
                        tPoint.y = padding.top;
                        break;
                }
				FCRect tRect = {tPoint.x, tPoint.y, tPoint.x + tSize.cx, tPoint.x + tSize.cy};
				if(autoEllipsis() && (tRect.right < clipRect.right || tRect.bottom < clipRect.bottom)){
					if(tRect.right < clipRect.right){
						tRect.right = clipRect.right;
					}
					if(tRect.bottom < clipRect.bottom){
						tRect.bottom = clipRect.bottom;
					}
					paint->drawTextAutoEllipsis(text.c_str(), linkColor, font, tRect);
				}
				else{
					paint->drawText(text.c_str(), linkColor, font, tRect);
				}
				FCNative *native = getNative();
				if (m_linkBehavior == LinkBehaviorA_AlwaysUnderLine || (m_linkBehavior == LinkBehaviorA_HoverUnderLine && (this != native->getHoveredControl() && this != native->getPushedControl()))){
					paint->drawLine(linkColor, 1, 0, tPoint.x, tPoint.y + tSize.cy, tPoint.x + tSize.cx, tPoint.y + tSize.cy);
				}
			}
		}
	}

	void FCLinkLabel::setProperty(const String& name, const String& value){
		if (name == L"activelinkcolor"){
			setActiveLinkColor(FCStr::convertStrToColor(value));
		}
		else if (name == L"disabledlinkcolor"){
			setDisabledLinkColor(FCStr::convertStrToColor(value));
		}
		else if (name == L"linkbehavior"){
			String lowerStr = FCStr::toLower(value);
			if (lowerStr == L"alwaysunderline"){
				setLinkBehavior(LinkBehaviorA_AlwaysUnderLine);
			}
			else if (lowerStr == L"hoverunderline"){
				setLinkBehavior(LinkBehaviorA_HoverUnderLine);
			}
			else{
				setLinkBehavior(LinkBehaviorA_NeverUnderLine);
			}
		}
		else if (name == L"linkcolor"){
			setLinkColor(FCStr::convertStrToColor(value));
		}
		else if (name == L"linkvisited"){
			setLinkVisited(FCStr::convertStrToBool(value));
		}
		else if (name == L"visitedlinkcolor"){
			setVisitedLinkColor(FCStr::convertStrToColor(value));
		}
		else{
			FCLabel::setProperty(name, value);
		}
	}
}