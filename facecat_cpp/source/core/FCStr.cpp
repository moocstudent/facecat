#include "..\\..\\stdafx.h"
#include "..\\..\\include\\core\\FCStr.h"

namespace FaceCat{
	CodeConvert_Win::CodeConvert_Win( const char* input, unsigned int fromCodePage, unsigned int toCodePage ){
		int len = MultiByteToWideChar(fromCodePage, 0, input, -1, 0, 0);
		wcharBuf = new wchar_t[len + 1];
		memset(wcharBuf,0,sizeof(wchar_t)*(len + 1));
		MultiByteToWideChar(fromCodePage, 0, input, -1, wcharBuf, len);
		len = WideCharToMultiByte(toCodePage, 0, wcharBuf, -1, 0, 0, 0, 0);
		charBuf = new char[len + 1];
		memset(charBuf,0,sizeof(char)*(len + 1));
		WideCharToMultiByte(toCodePage, 0, wcharBuf, -1, charBuf, len, 0, 0);
	}

	double FCStr::round(double x){     
		int sa = 0, si = 0;  
		if(x == 0.0){
			return 0.0; 
		}
		else{
			if(x > 0.0){
				sa = (int)x;   
				si = (int)(x + 0.5);        
				if(sa == floor((double)si)){
					return sa;    
				}
				else{
					return sa + 1; 
				}
			}       
			else{
				sa = (int)x;   
				si = (int)(x - 0.5);      
				if(sa == ceil((double)si)){
					return sa;       
				}
				else{
					return sa - 1;      
				}
			}
		}
	}

	void FCStr::ANSCToUnicode(string& out, const char* inSrc){
		if (!inSrc){
			return ;
		}
		out = CodeConvert_Win(inSrc, CP_UTF8, CP_ACP).toString();
	}

	void FCStr::contact(wchar_t *str, const wchar_t *str1, const wchar_t *str2, const wchar_t *str3){
		str[0] = _T('\0');
		lstrcat(str, str1);
		if(lstrlen(str2) > 0){
			lstrcat(str, str2);
		}
		if(lstrlen(str3) > 0){
			lstrcat(str, str3);
		}
	}

	String FCStr::convertAnchorToStr(const FCAnchor& anchor){
		vector<String> strs;
		if(anchor.left){
			strs.push_back(L"Left");
		}
		if(anchor.top){
			strs.push_back(L"Top");
		}
		if(anchor.right){
			strs.push_back(L"Right");
		}
		if(anchor.bottom){
			strs.push_back(L"Bottom");
		}
		String anchorStr;
		int size = (int)strs.size();
		if(size > 0){
			for(int i = 0; i < size; i++){
				anchorStr += strs[i];
				if(i != size - 1){
					anchorStr += L",";
				}
			}
		}
		else{
			anchorStr = L"None";
		}
		return anchorStr;
	}

	String FCStr::convertBoolToStr(bool value){
		return value ? L"True" : L"False";
	}

	String FCStr::convertColorToStr(Long color){
		if(color == FCColor_None){
			return L"Empty";
		}
        else if (color == FCColor_Back){
            return L"Control";
        }
        else if (color == FCColor_Border){
            return L"ControlBorder";
        }
        else if (color == FCColor_Text){
            return L"ControlText";
        }
        else if (color == FCColor_DisabledBack){
            return L"DisabledControl";
        }
        else if (color == FCColor_DisabledText){
            return L"DisabledControlText";
        }
		int a = 0, r = 0, g = 0, b = 0;
		FCColor::toArgb(0, color, &a, &r, &g, &b);
		wchar_t str[100] = {0};
		if(a == 255){
			_stprintf_s(str, 99, L"%d,%d,%d", r, g, b);
		}
		else{
			_stprintf_s(str, 99, L"%d,%d,%d,%d", a, r, g, b);
		}
		return str;
	}

	String FCStr::convertContentAlignmentToStr(FCContentAlignment contentAlignment){
		String str;
        if (contentAlignment == FCContentAlignment_BottomCenter){
            str = L"BottomCenter";
        }
        else if (contentAlignment == FCContentAlignment_BottomLeft){
            str = L"BottomLeft";
        }
        else if (contentAlignment == FCContentAlignment_BottomRight){
            str = L"BottomRight";
        }
        else if (contentAlignment == FCContentAlignment_MiddleCenter){
            str = L"MiddleCenter";
        }
        else if (contentAlignment == FCContentAlignment_MiddleLeft){
            str = L"MiddleLeft";
        }
        else if (contentAlignment == FCContentAlignment_MiddleRight){
            str = L"MiddleRight";
        }
        else if (contentAlignment == FCContentAlignment_TopCenter){
            str = L"TopCenter";
        }
        else if (contentAlignment == FCContentAlignment_TopLeft){
            str = L"TopLeft";
        }
        else if (contentAlignment == FCContentAlignment_TopRight){
            str = L"TopRight";
        }
		return str;
	}

    String FCStr::convertCursorToStr(FCCursors cursor){
        String str;
        if (cursor == FCCursors_AppStarting){
            str = L"AppStarting";
        }
        else if (cursor == FCCursors_Arrow){
            str = L"Arrow";
        }
        else if (cursor == FCCursors_Cross){
            str = L"Cross";
        }
        else if (cursor == FCCursors_Hand){
            str = L"Hand";
        }
        else if (cursor == FCCursors_Help){
            str = L"Help";
        }
        else if (cursor == FCCursors_IBeam){
            str = L"IBeam";
        }
        else if (cursor == FCCursors_No){
            str = L"No";
        }
        else if (cursor == FCCursors_SizeAll){
            str = L"SizeAll";
        }
        else if (cursor == FCCursors_SizeNESW){
            str = L"SizeNESW";
        }
        else if (cursor == FCCursors_SizeNS){
            str = L"SizeNS";
        }
        else if (cursor == FCCursors_SizeNWSE){
            str = L"SizeNWSE";
        }
        else if (cursor == FCCursors_SizeWE){
            str = L"SizeWE";
        }
        else if (cursor == FCCursors_UpArrow){
            str = L"UpArrow";
        }
        else if (cursor == FCCursors_WaitCursor){
            str = L"WaitCursor";
        }
        return str;
    }

	String FCStr::convertDockToStr(FCDockStyle dock){
		String str;
		if(dock == FCDockStyle_Bottom){
			str = L"Bottom";
		}
		else if(dock == FCDockStyle_Fill){
			str = L"Fill";
		}
		else if(dock == FCDockStyle_Left){
			str = L"Left";
		}
		else if(dock == FCDockStyle_None){
			str = L"None";
		}
		else if(dock == FCDockStyle_Right){
			str = L"Right";
		}
		else if(dock == FCDockStyle_Top){
			str = L"Top";
		}
		return str;
	}

	String FCStr::convertDoubleToStr(double value){
		wchar_t str[100] = {0};
		_stprintf_s(str, 99, L"%f", value);
		return str;
	}

	String FCStr::convertFloatToStr(float value){
		wchar_t str[100] = {0};
		_stprintf_s(str, 99, L"%f", value);
		return str;
	}

	String FCStr::convertFontToStr(FCFont *font){
		vector<String> strs;
		strs.push_back(font->m_fontFamily);
		wchar_t strFontSize[20] = {0};
		_stprintf_s(strFontSize, 19, L"%f", font->m_fontSize);
		strs.push_back(strFontSize);
		if(font->m_bold){
			strs.push_back(L"Bold");
		}
		if(font->m_underline){
			strs.push_back(L"UnderLine");
		}
		if(font->m_italic){
			strs.push_back(L"Italic");
		}
		if(font->m_strikeout){
			strs.push_back(L"Strikeout");
		}
		String fontStr;
		int size = (int)strs.size();
		if(size > 0){
			for(int i = 0; i < size; i++){
				fontStr += strs[i];
				if(i != size - 1){
					fontStr += L",";
				}
			}
		}
		return fontStr;
	}

	String FCStr::convertHorizontalAlignToStr(FCHorizontalAlign horizontalAlign){
	    String str;
        if (horizontalAlign == FCHorizontalAlign_Center){
            str = L"Center";
        }
        else if (horizontalAlign == FCHorizontalAlign_Right){
            str = L"Right";
        }
		else if (horizontalAlign == FCHorizontalAlign_Inherit){
            str = L"Inherit";
        }
        else if (horizontalAlign == FCHorizontalAlign_Left){
            str = L"Left";
        }
        return str;
	}

	String FCStr::convertIntToStr(int value){
		wchar_t str[100] = {0};
		_stprintf_s(str, 99, L"%d", value);
		return str;
	}

	String FCStr::convertLayoutStyleToStr(FCLayoutStyle layoutStyle){
		String str;
        if (layoutStyle == FCLayoutStyle_BottomToTop){
            str = L"BottomToTop";
        }
        else if (layoutStyle == FCLayoutStyle_LeftToRight){
            str = L"LeftToRight";
        }
        else if (layoutStyle == FCLayoutStyle_None){
            str = L"None";
        }
        else if (layoutStyle == FCLayoutStyle_RightToLeft){
            str = L"RightToLeft";
        }
        else if (layoutStyle == FCLayoutStyle_TopToBottom){
            str = L"TopToBottom";
        }
        return str;
	}

	String FCStr::convertLongToStr(Long value){
		wchar_t str[100] = {0};
		_stprintf_s(str, 99, L"%ld", value);
		return str;
	}

	String FCStr::convertPaddingToStr(const FCPadding& padding){
		wchar_t str[100] = {0};
		_stprintf_s(str, 99, L"%d,%d,%d,%d", padding.left, padding.top, padding.right, padding.bottom);
		return str;
	}

	String FCStr::convertPointToStr(const FCPoint& mp){
		wchar_t str[100] = {0};
		_stprintf_s(str, 99, L"%d,%d", mp.x, mp.y);
		return str;
	}

	String FCStr::convertRectToStr(const FCRect& rect){
		wchar_t str[100] = {0};
		_stprintf_s(str, 99, L"%d,%d,%d,%d", rect.left, rect.top, rect.right, rect.bottom);
		return str;
	}

	String FCStr::convertSizeToStr(const FCSize& size){
		wchar_t str[100] = {0};
		_stprintf_s(str, 99, L"%d,%d", size.cx, size.cy);
		return str;
	}

	FCAnchor FCStr::convertStrToAnchor(const String& str){
		String lowerStr = toLower(str);
		bool left = false, top = false, right = false, bottom = false;
		ArrayList<String> strs = FCStr::split(lowerStr, L",");
		int strSize = (int)strs.size();
		for(int i = 0; i < strSize; i++){
			String str = strs.get(i);
			if(str == L"left"){
				left = true;
			}
			else if(str == L"top"){
				top = true;
			}
			else if(str == L"right"){
				right = true;
			}
			else if(str == L"bottom"){
				bottom =true;
			}
		}
		FCAnchor anchor(left, top, right, bottom);
		return anchor;
	}

	bool FCStr::convertStrToBool(const String& str){
		String lowerStr = toLower(str);
		return lowerStr == L"true" ? true : false;
	}

	Long FCStr::convertStrToColor(const String& str){
		String lowerStr = toLower(str);
	    if (lowerStr == L"empty"){
            return FCColor_None;
        }
        else if (lowerStr == L"control"){
            return FCColor_Back;
        }
        else if (lowerStr == L"controlborder"){
            return FCColor_Border;
        }
        else if (lowerStr == L"controltext"){
            return FCColor_Text;
        }
        else if (lowerStr == L"disabledcontrol"){
            return FCColor_DisabledBack;
        }
        else if (lowerStr == L"disabledcontroltext"){
            return FCColor_DisabledText;
        }
		else{
			int a = 255, r = 255, g = 255, b = 255;
			ArrayList<String> strs = FCStr::split(str, L",");
			if(strs.size() == 3){
				r = _wtoi(strs.get(0).c_str());
				g = _wtoi(strs.get(1).c_str());
				b = _wtoi(strs.get(2).c_str());
				return FCColor::argb(r, g, b);
			}
			else if(strs.size() == 4){
				a = _wtoi(strs.get(0).c_str());
				r = _wtoi(strs.get(1).c_str());
				g = _wtoi(strs.get(2).c_str());
				b = _wtoi(strs.get(3).c_str());
				return FCColor::argb(a, r, g, b);
			}
		}
		return FCColor_None;
	}

	FCContentAlignment FCStr::convertStrToContentAlignment(const String& str){
		String lowerStr = toLower(str);
		FCContentAlignment textAlignment = FCContentAlignment_MiddleCenter;
	    if (lowerStr == L"bottomcenter"){
            textAlignment = FCContentAlignment_BottomCenter;
        }
        else if (lowerStr == L"bottomleft"){
            textAlignment = FCContentAlignment_BottomLeft;
        }
        else if (lowerStr == L"bottomright"){
            textAlignment = FCContentAlignment_BottomRight;
        }
        else if (lowerStr == L"middlecenter"){
            textAlignment = FCContentAlignment_MiddleCenter;
        }
        else if (lowerStr == L"middleleft"){
            textAlignment = FCContentAlignment_MiddleLeft;
        }
        else if (lowerStr == L"middleright"){
            textAlignment = FCContentAlignment_MiddleRight;
        }
        else if (lowerStr == L"topcenter"){
            textAlignment = FCContentAlignment_TopCenter;
        }
        else if (lowerStr == L"topleft"){
            textAlignment = FCContentAlignment_TopLeft;
        }
        else if (lowerStr == L"topright"){
            textAlignment = FCContentAlignment_TopRight;
        }
		return textAlignment;
	}

    FCCursors FCStr::convertStrToCursor(const String& str){
		String lowerStr = toLower(str);
        FCCursors cursor = FCCursors_Arrow;
        if (lowerStr == L"appstarting"){
			cursor = FCCursors_AppStarting;
        }
        else if (lowerStr == L"cross"){
            cursor = FCCursors_Cross;
        }
        else if (lowerStr == L"hand"){
            cursor = FCCursors_Hand;
        }
        else if (lowerStr == L"help"){
            cursor = FCCursors_Help;
        }
        else if (lowerStr == L"ibeam"){
            cursor = FCCursors_IBeam;
        }
        else if (lowerStr == L"no"){
            cursor = FCCursors_No;
        }
        else if (lowerStr == L"sizeall"){
            cursor = FCCursors_SizeAll;
        }
        else if (lowerStr == L"sizenesw"){
            cursor = FCCursors_SizeNESW;
        }
        else if (lowerStr == L"sizens"){
            cursor = FCCursors_SizeNS;
        }
        else if (lowerStr == L"sizenwse"){
            cursor = FCCursors_SizeNWSE;
        }
        else if (lowerStr == L"sizewe"){
            cursor = FCCursors_SizeWE;
        }
        else if (lowerStr == L"uparrow"){
            cursor = FCCursors_UpArrow;
        }
        else if (lowerStr == L"waitcursor"){
            cursor = FCCursors_WaitCursor;
        }
        return cursor;
    }

	FCDockStyle FCStr::convertStrToDock(const String& str){
		String lowerStr = toLower(str);
        FCDockStyle dock = FCDockStyle_None;
        if (lowerStr == L"bottom"){
            dock = FCDockStyle_Bottom;
        }
        else if (lowerStr == L"fill"){
            dock = FCDockStyle_Fill;
        }
        else if (lowerStr == L"left"){
            dock = FCDockStyle_Left;
        }
        else if (lowerStr == L"right"){
            dock = FCDockStyle_Right;
        }
        else if (lowerStr == L"top"){
            dock = FCDockStyle_Top;
        }
        return dock;
    }

	double FCStr::convertStrToDouble(const String& str){
		return _wtof(str.c_str());
	}

	double FCStr::convertStrToDouble(const wchar_t *str){
		return _wtof(str);
	}

	float FCStr::convertStrToFloat(const String& str){
		return (float)_wtof(str.c_str());
	}

	FCFont* FCStr::convertStrToFont(const String& str){
		ArrayList<String> strs = FCStr::split(str, L",");
		int size = (int)strs.size();
		String fontFamily = L"SimSun";
		float fontSize = 12;
		bool bold = false;
		bool underline = false;
		bool italic = false;
		bool strikeout = false;
		if(size >= 1){
			fontFamily = strs.get(0);
		}
		if(size >= 2){
			fontSize = (float)_wtof(strs.get(1).c_str());
		}		
		for(int i = 2; i < size; i++){
			String subStr = toLower(strs.get(i));
			if(subStr == L"bold"){
				bold = true;
			}
			else if(subStr == L"underline"){
				underline = true;
			}
			else if(subStr == L"italic"){
				italic = true;
			}
			else if(subStr == L"strikeout"){
				strikeout = true;
			}
		}
		return new FCFont(fontFamily, fontSize, bold, underline, italic, strikeout);	
	}

	FCHorizontalAlign FCStr::convertStrToHorizontalAlign(const String& str){
		String lowerStr = toLower(str);
	    FCHorizontalAlign horizontalAlign = FCHorizontalAlign_Center;
        if (lowerStr == L"right"){
            horizontalAlign = FCHorizontalAlign_Right;
        }
		else if(lowerStr == L"inherit"){
			horizontalAlign = FCHorizontalAlign_Inherit;
		}
        else if (lowerStr == L"left"){
            horizontalAlign = FCHorizontalAlign_Left;
        }
        return horizontalAlign;
	}

	int FCStr::convertStrToInt(const String& str){
		return _wtoi(str.c_str());
	}

	int FCStr::convertStrToInt(const wchar_t *str){
		return _wtoi(str);
	}

	FCLayoutStyle FCStr::convertStrToLayoutStyle(const String& str){
		String lowerStr = toLower(str);
        FCLayoutStyle layoutStyle = FCLayoutStyle_None;
        if (lowerStr == L"bottomtotop"){
            layoutStyle = FCLayoutStyle_BottomToTop;
        }
        else if (lowerStr == L"lefttoright"){
            layoutStyle = FCLayoutStyle_LeftToRight;
        }
        else if (lowerStr == L"righttoleft"){
            layoutStyle = FCLayoutStyle_RightToLeft;
        }
        else if (lowerStr == L"toptobottom"){
            layoutStyle = FCLayoutStyle_TopToBottom;
        }
        return layoutStyle;
    }

	Long FCStr::convertStrToLong(const String& str){
		return _wtol(str.c_str());
	}

	Long FCStr::convertStrToLong(const wchar_t *str){
		return _wtol(str);
	}

	FCPadding FCStr::convertStrToPadding(const String& str){
		int left = 0, top = 0, right = 0, bottom = 0;
		ArrayList<String> strs = FCStr::split(str, L",");
		if(strs.size() == 4){
			left = _wtoi(strs.get(0).c_str());
			top = _wtoi(strs.get(1).c_str());
			right = _wtoi(strs.get(2).c_str());
			bottom = _wtoi(strs.get(3).c_str());
		}
		FCPadding padding(left, top, right, bottom);
		return padding;
	}

	FCPoint FCStr::convertStrToPoint(const String& str){
		int x = 0, y = 0;
		ArrayList<String> strs = FCStr::split(str, L",");
		if(strs.size() == 2){
			x = _wtoi(strs.get(0).c_str());
			y = _wtoi(strs.get(1).c_str());
		}
		FCPoint mp = {x, y};
		return mp;
	}

	FCRect FCStr::convertStrToRect(const String& str){
		int left = 0, top = 0, right = 0, bottom = 0;
		ArrayList<String> strs = FCStr::split(str, L",");
		if(strs.size() == 4){
			left = _wtoi(strs.get(0).c_str());
			top = _wtoi(strs.get(1).c_str());
			right = _wtoi(strs.get(2).c_str());
			bottom = _wtoi(strs.get(3).c_str());
		}
		FCRect rect = {left, top, right, bottom};
		return rect;	
	}

	FCSize FCStr::convertStrToSize(const String& str){
		int cx = 0, cy = 0;
		ArrayList<String> strs = FCStr::split(str, L",");
		if(strs.size() == 2){
			cx = _wtoi(strs.get(0).c_str());
			cy = _wtoi(strs.get(1).c_str());
		}
		FCSize size = {cx, cy};
		return size;
	}

	FCVerticalAlign FCStr::convertStrToVerticalAlign(const String& str){
		String lowerStr = toLower(str);
        FCVerticalAlign verticalAlign = FCVerticalAlign_Middle;
        if (str == L"bottom"){
            verticalAlign = FCVerticalAlign_Bottom;
        }
        else if (str == L"inherit"){
            verticalAlign = FCVerticalAlign_Inherit;
        }
        else if (str == L"top"){
            verticalAlign = FCVerticalAlign_Top;
        }
        return verticalAlign;
	}

	String FCStr::convertVerticalAlignToStr(FCVerticalAlign verticalAlign){
		String str;
        if (verticalAlign == FCVerticalAlign_Bottom){
            str = L"Bottom";
        }
        else if (verticalAlign == FCVerticalAlign_Inherit){
            str = L"Inherit";
        }
        else if (verticalAlign == FCVerticalAlign_Middle){
            str = L"Middle";
        }
        else if (verticalAlign == FCVerticalAlign_Top){
            str = L"Top";
        }
        return str;
	}

	string FCStr::getAppPath(){
		char exeFullPath[MAX_PATH]; 
		string strPath = "";
		GetModuleFileNameA(0, exeFullPath, MAX_PATH);
		strPath = (string)exeFullPath; 
		int pos = (int)strPath.rfind('\\', strPath.length());
		return strPath.substr(0, pos);
	}

	    inline int isLeapYead(int year){
        return( (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0));
    }
    
    const int evenyMonths[2][12] = {{31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31},{31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}
    };
    
    double FCStr::getDateNum(int tm_year, int tm_mon, int tm_mday, int tm_hour, int tm_min, int tm_sec, int tm_msec){
        double tn = 0;
        if ((tm_mon -= 2) <= 0){
            tm_mon += 12;
            tm_year -= 1;
        }
        double year = tm_year / 4 - tm_year / 100 + tm_year / 400 + 367 * tm_mon / 12 + tm_mday;
        double month = tm_year * 365 - 719499;
        tn = ((year + month) * 24 * 60 * 60) + tm_hour * 3600 + tm_min * 60 + tm_sec;
        return (double)((long long)tn + tm_msec / 1000);
    }
    
    void FCStr::getDateByNum(double num, int *tm_year, int *tm_mon, int *tm_mday, int *tm_hour, int *tm_min, int *tm_sec, int *tm_msec){
        long tn = (long)num;
        int dayclock = (int)(tn % 86400);
        int dayno = (int)(tn / 86400);
        int year = 1970;
        *tm_sec = dayclock % 60;
        *tm_min = (dayclock % 3600) / 60;
        *tm_hour = dayclock / 3600;
        int yearSize = 0;
        while (dayno >= (yearSize = (isLeapYead(year) == 1 ? 366 : 365))){
            dayno -= yearSize;
            year++;
        }
        *tm_year = year;
        int month = 0;
        while (dayno >= evenyMonths[isLeapYead(year)][month]){
            dayno -= evenyMonths[isLeapYead(year)][month];
            month++;
        }
        *tm_mon = month + 1;
        *tm_mday = dayno + 1;
        *tm_msec = (long)((num * 1000 - floor(num) * 1000));
    }

	string FCStr::getGuid(){
		static char buf[64] = {0};
		GUID guid;
		if (S_OK == ::CoCreateGuid(&guid)){
			_snprintf_s(buf, sizeof(buf)
				, "{%08X-%04X-%04x-%02X%02X-%02X%02X%02X%02X%02X%02X}"
				, guid.Data1
				, guid.Data2
				, guid.Data3
				, guid.Data4[0], guid.Data4[1]
			, guid.Data4[2], guid.Data4[3], guid.Data4[4], guid.Data4[5]
			, guid.Data4[6], guid.Data4[7]
			);
		}
		return buf;
	}

	void FCStr::getFormatDate(double date, wchar_t *str){
		int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, msecond = 0;
		getDateByNum(date, &year, &month, &day, &hour, &minute, &second, &msecond);
		if(hour != 0){
			_stprintf_s(str, 99, L"%02d:%02d", hour, minute);
		}
		else{
			_stprintf_s(str, 99, L"%d-%02d-%02d", year, month, day);
		}
	}

	String FCStr::getFormatDate(const String& format, int year, int month, int day, int hour, int minute, int second){
		String strDate = format;
		wchar_t sz[100] = {0};
		_stprintf_s(sz, 99, L"%d", year);
		String strYear = sz;
		memset(sz, '\0', 100);
		_stprintf_s(sz, 99, L"%02d", month);
		String strMonth = sz;
		memset(sz, '\0', 100);
		_stprintf_s(sz, 99, L"%02d", day);
		String strDay = sz;
		memset(sz, '\0', 100);
		_stprintf_s(sz, 99, L"%d", day);
		String strHour = sz;
		memset(sz, '\0', 100);
		_stprintf_s(sz, 99, L"%02d", minute);
		String strMinute = sz;
		memset(sz, '\0', 100);
		_stprintf_s(sz, 99, L"%02d", second);
		String strSecond = sz;
		if((int)format.find(L"yyyy") != -1){
			strDate = FCStr::replace(strDate, L"yyyy", strYear);
		}
		if((int)format.find(L"MM") != -1){
			strDate = FCStr::replace(strDate, L"MM", strMonth);
		}
		if((int)format.find(L"dd") != -1){
			strDate = FCStr::replace(strDate, L"dd", strDay);
		}
		if((int)format.find(L"HH") != -1){
			strDate = FCStr::replace(strDate, L"HH", strHour);
		}
		if((int)format.find(L"mm") != -1){
			strDate = FCStr::replace(strDate, L"mm", strMinute);
		}
		if((int)format.find(L"ss") != -1){
			strDate = FCStr::replace(strDate, L"ss", strSecond);
		}
		return strDate;
	}

	void FCStr::getValueByDigit(double value, int digit, wchar_t *str){
		if(!_isnan(value)){
			if(digit == 0){
				double newValue = round(value);
				if(abs(value - newValue) < 1){
					value = newValue;
				}
			}
			switch(digit){
			case 0:
				_stprintf_s(str, 99, L"%d", (int)value);
				break;
			case 1:
				_stprintf_s(str, 99, L"%.1f", value);
				break;
			case 2:
				_stprintf_s(str, 99, L"%.2f", value);
				break;
			case 3:
				_stprintf_s(str, 99, L"%.3f", value);
				break;
			case 4:
				_stprintf_s(str, 99, L"%.4f", value);
				break;
			case 5:
				_stprintf_s(str, 99, L"%.5f", value);
				break;
			case 6:
				_stprintf_s(str, 99, L"%.6f", value);
				break;
			case 7:
				_stprintf_s(str, 99, L"%.7f", value);
				break;
			case 8:
				_stprintf_s(str, 99, L"%.8f", value);
				break;
			case 9:
				_stprintf_s(str, 99, L"%.9f", value);
				break;
			default:
				_stprintf_s(str, 99, L"%f", value);
			}
		}
		str = 0;
	}

	int FCStr::hexToDec(const char *str){
		bool IsHex = false;
		int result = 0;
		int i = 0, szLen = 0, nHex;
		if(str[0] == '0'){
			if((str[1] == 'x') || (str[1] == 'X')){
				IsHex = true;
				i = 2;
			}
		}
	    szLen = strlen(str);
	    nHex = '0';
	    for (; i < szLen; i++){
			if(IsHex){
				if ((str[i] >= '0') && (str[i] <= '9')){
					nHex = '0';
				}
				else if ((str[i] >= 'A') && (str[i] <= 'F')){
					nHex = 'A' - 10;
				}
		        else if ((str[i] >= 'a') && (str[i] <= 'f')){
					nHex = 'a' - 10;
				}
				else{
					return 0;
				}
				result = result * 16 + str[i] - nHex;
			}
			else{
				if ((str[i] < '0') || (str[i] > '9')){
					return 0;
				}
				result = result * 10 + str[i] - nHex;
			}
		}
		return result;
	}

	string FCStr::replace(const string& str, const string& src, const string& dest)
	{
		string newStr = str;
		std::string::size_type pos = 0;
		while( (pos = newStr.find(src, pos)) != string::npos )
		{
			newStr.replace(pos, src.length(), dest);
			pos += dest.length();
		}
		return newStr;
	}

	String FCStr::replace(const String& str, const String& src, const String& dest){
		String newStr = str;
		String::size_type pos = 0;
		while( (pos = newStr.find(src, pos)) != string::npos ){
			newStr.replace(pos, src.length(), dest);
			pos += dest.length();
		}
		return newStr;
	}

	float FCStr::safeDoubleToFloat(double value, int digit){
		wchar_t szValue[100] = {0};
		getValueByDigit(value, digit, szValue);
		return convertStrToFloat(szValue);
	}

	ArrayList<String> FCStr::split(const String& str, const String& pattern){
		 int pos = -1;
		 ArrayList<String> result;
		 String newStr = str;
		 newStr += pattern;
		 int size = (int)str.size();
		 for(int i = 0; i < size; i++){
			pos = (int)newStr.find(pattern, i);
			if((int)pos <= size){
				String s = newStr.substr(i, pos - i);
				if(s.length() > 0){
					result.add(s);
				}
				i = pos + (int)pattern.size() - 1;
			}
		}
		return result;
	}

	String FCStr::stringTowstring(const string& strSrc){
		int  unicodeLen = ::MultiByteToWideChar(CP_ACP, 0, strSrc.c_str(), -1, 0, 0);
		wchar_t *pUnicode = new  wchar_t[unicodeLen + 1];
		memset(pUnicode,0,(unicodeLen + 1) * sizeof(wchar_t));
		::MultiByteToWideChar(CP_ACP, 0, strSrc.c_str(), - 1, (LPWSTR)pUnicode, unicodeLen);
		String strDest = ( wchar_t* )pUnicode;
		delete[] pUnicode;
		return strDest;
	}

	String FCStr::toLower(const String& str){
		String lowerStr = str;
		transform(lowerStr.begin(), lowerStr.end(), lowerStr.begin(), tolower);
		return lowerStr;
	}

	String FCStr::toUpper(const String& str){
		String upperStr = str;
		transform(upperStr.begin(), upperStr.end(), upperStr.begin(), toupper);
		return upperStr;
	}

	void FCStr::unicodeToANSC(string& out, const char* inSrc){
		if (!inSrc){
			return ;
		}
		out = CodeConvert_Win(inSrc, CP_ACP, CP_UTF8).toString();
	}

	string FCStr::urlEncode(const std::string& szToEncode)
	{
		string src = szToEncode;  
		char hex[] = "0123456789ABCDEF";  
		string dst; 
		for (size_t i = 0; i < src.size(); ++i)  
		{  
			unsigned char cc = src[i];  
			if ( cc >= 'A' && cc <= 'Z'   
					 || cc >='a' && cc <= 'z'  
					 || cc >='0' && cc <= '9'  
					 || cc == '.'  
					 || cc == '_'  
					 || cc == '-'  
					 || cc == '*')  
			{  
				if (cc == ' ')  
				{  
					dst += "+";  
				}  
				else  
					dst += cc;  
			}  
			else  
			{  
				unsigned char c = static_cast<unsigned char>(src[i]);  
				dst += '%';  
				dst += hex[c / 16];  
				dst += hex[c % 16];  
			}  
		}  
		return dst;  
	}

	string FCStr::urlDecode(const std::string& szToDecode)
	{
		string result;  
		int hex = 0;  
		for (size_t i = 0; i < szToDecode.length(); ++i)  
		{  
			switch (szToDecode[i])  
			{  
			case '+':  
				result += ' ';  
				break;  
			case '%':  
				if (isxdigit(szToDecode[i + 1]) && isxdigit(szToDecode[i + 2]))  
				{  
					std::string hexStr = szToDecode.substr(i + 1, 2);  
					hex = strtol(hexStr.c_str(), 0, 16);  
					//字母和数字[0-9a-zA-Z]、一些特殊符号[$-_.+!*'(),] 、以及某些保留字[$&+,/:;=?@]  
					//可以不经过编码直接用于URL  
					if (!((hex >= 48 && hex <= 57) || //0-9  
						(hex >=97 && hex <= 122) ||   //a-z  
						(hex >=65 && hex <= 90) ||    //A-Z  
						//一些特殊符号及保留字[$-_.+!*'(),]  [$&+,/:;=?@]  
						hex == 0x21 || hex == 0x24 || hex == 0x26 || hex == 0x27 || hex == 0x28 || hex == 0x29  
						|| hex == 0x2a || hex == 0x2b|| hex == 0x2c || hex == 0x2d || hex == 0x2e || hex == 0x2f  
						|| hex == 0x3A || hex == 0x3B|| hex == 0x3D || hex == 0x3f || hex == 0x40 || hex == 0x5f  
						))  
					{  
						result += char(hex);  
						i += 2;  
					}  
					else result += '%';  
				}else {  
					result += '%';  
				}  
				break;  
			default:  
				result += szToDecode[i];  
				break;  
			}  
		}  
		return result;  
	}

	string FCStr::wstringTostring(const String& strSrc){
		int iTextLen = WideCharToMultiByte(CP_ACP, 0, strSrc.c_str(), -1, 0, 0, 0, 0);
		char *pElementText = new char[iTextLen + 1];
		memset( ( void* )pElementText, 0, sizeof( char ) * ( iTextLen + 1 ) );
		::WideCharToMultiByte(CP_ACP, 0, strSrc.c_str(), - 1, pElementText, iTextLen, 0, 0);
		string strDest = pElementText;
		delete[] pElementText;
		return strDest;
	}
}