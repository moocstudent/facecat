#include "..\\..\\stdafx.h"
#include "..\\..\\include\\core\\FCHost.h"

namespace FaceCat{
	FCHost::FCHost(){
	}

	FCHost::~FCHost(){
	}

	FCNative* FCHost::getNative(){
		return 0;
	}

	void FCHost::setNative(FCNative *native){

	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void FCHost::activeMirror(){
	}

	bool FCHost::allowOperate(){
		return true;
	}

	bool FCHost::allowPartialPaint(){
		return true;
	}

	void FCHost::beginInvoke(FCView *control, Object args){
	}

	void FCHost::copy(string text){
	}

	FCView* FCHost::createInternalControl(FCView *parent, const String& clsid){
		return 0;
	}

	FCCursors FCHost::getCursor(){
		return FCCursors_Arrow;
	}

	int FCHost::getIntersectRect(LPRECT lpDestRect, const FCRect *lpSrc1Rect, const FCRect *lpSrc2Rect){
		return 0;
	}

	FCPoint FCHost::getTouchPoint(){
		FCPoint mp = {0};
		return mp;
	}

	FCSize FCHost::getSize(){
		FCSize size = {0};
		return size;
	}

	int FCHost::getUnionRect(LPRECT lpDestRect, const FCRect *lpSrc1Rect, const FCRect *lpSrc2Rect){
		return 0;
	}

	void FCHost::invalidate(){
	}

	void FCHost::invalidate(const FCRect& rect){
	}

	void FCHost::invoke(FCView *control, Object args){
	}

	bool FCHost::isKeyPress(char key){
		return false;
	}

	string FCHost::paste(){
		return "";
	}

	void FCHost::setAllowOperate(bool allowOperate){
	}

	void FCHost::setAllowPartialPaint(bool allowPartialPaint){
	}

	void FCHost::setCursor(FCCursors cursor){
	}

	void FCHost::showToolTip(const String& text, const FCPoint& mp){
	}

	void FCHost::startTimer(int timerID, int interval){
	}

	void FCHost::stopTimer(int timerID){
	}
}