#include "stdafx.h"
#include "FCStr.h"

namespace FaceCat{
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
    
    void FCStr::contact(wchar_t *str, const wchar_t *str1, const wchar_t *str2, const wchar_t *str3){
        str[0] = L'\0';
        wcscat(str, str1);
        if(wcslen(str2) > 0){
            wcscat(str, str2);
        }
        if(wcslen(str3) > 0){
            wcscat(str, str3);
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
        wchar_t str[100] ={0};
        if(a == 255){
            swprintf(str, 99, L"%d,%d,%d", r, g, b);
        }
        else{
            swprintf(str, 99, L"%d,%d,%d,%d", a, r, g, b);
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
        if (cursor == FCCursors_Cross){
            str = L"Cross";
        }
        else if (cursor == FCCursors_Hand){
            str = L"Hand";
        }
        else if (cursor == FCCursors_IBeam){
            str = L"IBeam";
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
        wchar_t str[100] ={0};
        swprintf(str, 99, L"%f", value);
        return str;
    }
    
    String FCStr::convertFloatToStr(float value){
        wchar_t str[100] ={0};
        swprintf(str, 99, L"%f", value);
        return str;
    }
    
    String FCStr::convertFontToStr(FCFont *font){
        vector<String> strs;
        strs.push_back(font->m_fontFamily);
        wchar_t strFontSize[20] ={0};
        swprintf(strFontSize, 19, L"%f", font->m_fontSize);
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
        wchar_t str[100] ={0};
        swprintf(str, 99, L"%d", value);
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
        wchar_t str[100] ={0};
        swprintf(str, 99, L"%ld", value);
        return str;
    }
    
    String FCStr::convertPaddingToStr(const FCPadding& padding){
        wchar_t str[100] ={0};
        swprintf(str, 99, L"%d,%d,%d,%d", padding.left, padding.top, padding.right, padding.bottom);
        return str;
    }
    
    String FCStr::convertPointToStr(const FCPoint& mp){
        wchar_t str[100] ={0};
        swprintf(str, 99, L"%d,%d", mp.x, mp.y);
        return str;
    }
    
    String FCStr::convertRectToStr(const FCRect& rect){
        wchar_t str[100] ={0};
        swprintf(str, 99, L"%d,%d,%d,%d", rect.left, rect.top, rect.right, rect.bottom);
        return str;
    }
    
    String FCStr::convertSizeToStr(const FCSize& size){
        wchar_t str[100] ={0};
        swprintf(str, 99, L"%d,%d", size.cx, size.cy);
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
                r = convertStrToInt(strs.get(0).c_str());
                g = convertStrToInt(strs.get(1).c_str());
                b = convertStrToInt(strs.get(2).c_str());
                return FCColor::argb(r, g, b);
            }
            else if(strs.size() == 4){
                a = convertStrToInt(strs.get(0).c_str());
                r = convertStrToInt(strs.get(1).c_str());
                g = convertStrToInt(strs.get(2).c_str());
                b = convertStrToInt(strs.get(3).c_str());
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
        if (lowerStr == L"cross"){
            cursor = FCCursors_Cross;
        }
        else if (lowerStr == L"hand"){
            cursor = FCCursors_Hand;
        }
        else if (lowerStr == L"ibeam"){
            cursor = FCCursors_IBeam;
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
        return wcstod(str.c_str(), 0);
    }
    
    double FCStr::convertStrToDouble(const wchar_t *str){
        return wcstod(str, 0);
    }
    
    float FCStr::convertStrToFloat(const String& str){
        return wcstof(str.c_str(), 0);
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
            fontSize = (float)convertStrToDouble(strs.get(1).c_str());
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
        return new FCFont(fontFamily, (float)fontSize, bold, underline, italic, strikeout);
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
        return wcstol(str.c_str(), 0, 0);
    }
    
    int FCStr::convertStrToInt(const wchar_t *str){
        return wcstol(str, 0, 0);
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
        return wcstol(str.c_str(), 0, 0);
    }
    
    Long FCStr::convertStrToLong(const wchar_t *str){
        return wcstol(str, 0, 0);
    }
    
    FCPadding FCStr::convertStrToPadding(const String& str){
        int left = 0, top = 0, right = 0, bottom = 0;
        ArrayList<String> strs = FCStr::split(str, L",");
        if(strs.size() == 4){
            left = convertStrToInt(strs.get(0).c_str());
            top = convertStrToInt(strs.get(1).c_str());
            right = convertStrToInt(strs.get(2).c_str());
            bottom = convertStrToInt(strs.get(3).c_str());
        }
        FCPadding padding(left, top, right, bottom);
        return padding;
    }
    
    FCPoint FCStr::convertStrToPoint(const String& str){
        int x = 0, y = 0;
        ArrayList<String> strs = FCStr::split(str, L",");
        if(strs.size() == 2){
            x = convertStrToInt(strs.get(0).c_str());
            y = convertStrToInt(strs.get(1).c_str());
        }
        FCPoint mp ={x, y};
        return mp;
    }
    
    FCRect FCStr::convertStrToRect(const String& str){
        int left = 0, top = 0, right = 0, bottom = 0;
        ArrayList<String> strs = FCStr::split(str, L",");
        if(strs.size() == 4){
            left = convertStrToInt(strs.get(0).c_str());
            top = convertStrToInt(strs.get(1).c_str());
            right = convertStrToInt(strs.get(2).c_str());
            bottom = convertStrToInt(strs.get(3).c_str());
        }
        FCRect rect ={left, top, right, bottom};
        return rect;
    }
    
    FCSize FCStr::convertStrToSize(const String& str){
        int cx = 0, cy = 0;
        ArrayList<String> strs = FCStr::split(str, L",");
        if(strs.size() == 2){
            cx = convertStrToInt(strs.get(0).c_str());
            cy = convertStrToInt(strs.get(1).c_str());
        }
        FCSize size ={cx, cy};
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
        NSString *path = @"";
        NSString *str_app_full_file_name = [[NSBundle mainBundle] resourcePath];
        NSRange range = [str_app_full_file_name rangeOfString:@"/" options:NSBackwardsSearch];
        if (range.location != NSNotFound)
        {
            path = [str_app_full_file_name substringToIndex:range.location];
            path = [path stringByAppendingFormat:@"%@", @"/"];
        }
        string strPath = [path UTF8String];
        strPath = strPath.substr(0, (int)strPath.length() - 1);
        return strPath;
    }
    
    inline int isLeapYead(int year){
        return( (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0));
    }
    
    const int evenyMonths[2][12] ={{31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31},{31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}
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
        return (long long) tn + tm_msec / 1000;
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
    
    void FCStr::getValueByDigit(double value, int digit, wchar_t *str){
        if(!isnan(value)){
            if(digit == 0){
                double newValue = round(value);
                if(abs(value - newValue) < 1){
                    value = newValue;
                }
            }
            switch(digit){
                case 0:
                    swprintf(str, 99, L"%d", (int)value);
                    break;
                case 1:
                    swprintf(str, 99, L"%.1f", value);
                    break;
                case 2:
                    swprintf(str, 99, L"%.2f", value);
                    break;
                case 3:
                    swprintf(str, 99, L"%.3f", value);
                    break;
                case 4:
                    swprintf(str, 99, L"%.4f", value);
                    break;
                case 5:
                    swprintf(str, 99, L"%.5f", value);
                    break;
                case 6:
                    swprintf(str, 99, L"%.6f", value);
                    break;
                case 7:
                    swprintf(str, 99, L"%.7f", value);
                    break;
                case 8:
                    swprintf(str, 99, L"%.8f", value);
                    break;
                case 9:
                    swprintf(str, 99, L"%.9f", value);
                    break;
                default:
                    swprintf(str, 99, L"%f", value);
            }
        }
        str = 0;
    }
    
    void FCStr::getFormatDate(double date, wchar_t *str){
        int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, msecond = 0;
        getDateByNum(date, &year, &month, &day, &hour, &minute, &second, &msecond);
        if(hour != 0){
            swprintf(str, 99, L"%02d:%02d", hour, minute);
        }
        else{
            swprintf(str, 99, L"%d-%02d-%02d", year, month, day);
        }
    }
    
    String FCStr::getFormatDate(const String& format, int year, int month, int day, int hour, int minute, int second){
        String strDate = format;
        wchar_t sz[100] ={0};
        swprintf(sz, 99, L"%d", year);
        String strYear = sz;
        memset(sz, '\0', 100);
        swprintf(sz, 99, L"%02d", month);
        String strMonth = sz;
        memset(sz, '\0', 100);
        swprintf(sz, 99, L"%02d", day);
        String strDay = sz;
        memset(sz, '\0', 100);
        swprintf(sz, 99, L"%d", day);
        String strHour = sz;
        memset(sz, '\0', 100);
        swprintf(sz, 99, L"%02d", minute);
        String strMinute = sz;
        memset(sz, '\0', 100);
        swprintf(sz, 99, L"%02d", second);
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
    
    int FCStr::gZCompress(Byte *data, uLong ndata, Byte *zdata, uLong *nzdata){
        z_stream c_stream;
        int err = 0;
        if(data && ndata > 0){
            c_stream.zalloc = Z_NULL;
            c_stream.zfree = Z_NULL;
            c_stream.opaque = Z_NULL;
            if(deflateInit2(&c_stream, Z_DEFAULT_COMPRESSION, Z_DEFLATED,
                            MAX_WBITS + 16, 6, Z_DEFAULT_STRATEGY) != Z_OK){
                return -1;
            }
            c_stream.next_in  = data;
            c_stream.avail_in  = ndata;
            c_stream.next_out = zdata;
            c_stream.avail_out  = *nzdata;
            while(c_stream.avail_in != 0 && c_stream.total_out < *nzdata){
                if(deflate(&c_stream, Z_NO_FLUSH) != Z_OK){
                    return -1;
                }
            }
            if(c_stream.avail_in != 0){
                return c_stream.avail_in;
            }
            for(;;){
                if((err = deflate(&c_stream, Z_FINISH)) == Z_STREAM_END){
                    break;
                }
                if(err != Z_OK){
                    return -1;
                }
            }
            if(deflateEnd(&c_stream) != Z_OK){
                return -1;
            }
            *nzdata = c_stream.total_out;
            return 0;
        }
        return -1;
    }
    
    int FCStr::gZDeCompress(Byte *zdata, uLong nzdata, Byte *data, uLong *ndata){
        int err = 0;
        z_stream d_stream = {0};
        static char dummy_head[2] ={
            0x8 + 0x7 * 0x10,
            (((0x8 + 0x7 * 0x10) * 0x100 + 30) / 31 * 31) & 0xFF,
        };
        d_stream.zalloc = Z_NULL;
        d_stream.zfree = Z_NULL;
        d_stream.opaque = Z_NULL;
        d_stream.next_in  = zdata;
        d_stream.avail_in = 0;
        d_stream.next_out = data;
        if(inflateInit2(&d_stream, MAX_WBITS + 16) != Z_OK){
            return -1;
        }
    
        while(d_stream.total_out < *ndata && d_stream.total_in < nzdata){
            d_stream.avail_in = d_stream.avail_out = 1;
            if((err = inflate(&d_stream, Z_NO_FLUSH)) == Z_STREAM_END){
                break;
            }
            if(err != Z_OK){
                if(err == Z_DATA_ERROR){
                    d_stream.next_in = (Bytef*) dummy_head;
                    d_stream.avail_in = sizeof(dummy_head);
                    
                    if((err = inflate(&d_stream, Z_NO_FLUSH)) != Z_OK){
                        return -1;
                    }
                } else{
                    return -1;
                }
            }
        }
        if(inflateEnd(&d_stream) != Z_OK){
            return -1;
        }
        *ndata = d_stream.total_out;
        return 0;
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
        wchar_t szValue[100] ={0};
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
#if TARGET_RT_BIG_ENDIAN
        const NSStringEncoding kEncoding_wchar_t = cFStringConvertEncodingToNSStringEncoding(kCFStringEncodingUTF32BE);
#else
        const NSStringEncoding kEncoding_wchar_t = CFStringConvertEncodingToNSStringEncoding(kCFStringEncodingUTF32LE);
#endif
        @autoreleasepool{
            NSString *str = [NSString stringWithUTF8String:strSrc.c_str()];
            NSData *asData = [str dataUsingEncoding:kEncoding_wchar_t];
            return String((wchar_t*)[asData bytes], [asData length] / sizeof(wchar_t));
        }
    }
    
    String FCStr::toLower(const String& str){
        String lowerStr = str;
        transform(str.begin(), str.end(), lowerStr.begin(), ::tolower);
        return lowerStr;
    }
    
    String FCStr::toUpper(const String& str){
        String upperStr = str;
        transform(str.begin(), str.end(), upperStr.begin(), ::toupper);
        return upperStr;
    }
    
    string FCStr::wstringTostring(const String& strSrc){
#if TARGET_RT_BIG_ENDIAN
        const NSStringEncoding kEncoding_wchar_t = cFStringConvertEncodingToNSStringEncoding(kCFStringEncodingUTF32BE);
#else
        const NSStringEncoding kEncoding_wchar_t = CFStringConvertEncodingToNSStringEncoding(kCFStringEncodingUTF32LE);
#endif
        @autoreleasepool{
            char* data = (char*)strSrc.data();
            unsigned size = strSrc.size() * sizeof(wchar_t);
            NSString* result = [[NSString alloc] initWithBytes:data length:size encoding:kEncoding_wchar_t];
            return [result UTF8String];
        }
    }
}
