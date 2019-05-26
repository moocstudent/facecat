/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCSTR_H__
#define __FCSTR_H__
#pragma once
#include "..\\..\\stdafx.h"
#include <tchar.h>
#include "FCPaint.h"
#include "FCView.h"

namespace FaceCat{
	/*
	* 字符串转换
	*/
	class CodeConvert_Win{
	public:
		CodeConvert_Win(const char* input, unsigned int fromCodePage, unsigned int toCodePage);
		~CodeConvert_Win(){
			delete [] wcharBuf;        
			delete [] charBuf;    
			};    
			const char * toString(){    
				return charBuf;    
			};
	private:    
		  wchar_t * wcharBuf;    
		  char * charBuf;
	};

	/*
	* 字符串处理
	*/
	class FCStr{
	public:
		static double round(double x); 
	public:
		/*
		* 编码转换
		*/
		static void ANSCToUnicode(string& out, const char* inSrc);
		/*
		* 连接字符串
		*/
		static void contact(wchar_t *str, const wchar_t *str1, const wchar_t *str2, const wchar_t *str3);
	    /**
         * 锚定信息转化为字符
         * @param anchor  锚定信息
         * @returns 字符
        */
		static String convertAnchorToStr(const FCAnchor& anchor);
	    /**
         * 布尔型数值转换为字符
         * @param value   数值
         * @returns 字符
        */
		static String convertBoolToStr(bool value);
	    /**
         * 颜色转换为字符
         * @param color  颜色
         * @returns 字符
        */
		static String convertColorToStr(Long color);
	    /**
         * 内容布局转字符串
         * @param contentAlignment  内容布局
         * @returns 字符串
        */
		static String convertContentAlignmentToStr(FCContentAlignment contentAlignment);
		/*
		* 鼠标形状转为字符串
		*/
		static String convertCursorToStr(FCCursors cursor);
	    /**
         * 双精度浮点型数值转换为字符
         * @param value  数值
         * @returns 字符
        */
		static String convertDoubleToStr(double value);
	    /**
         * 绑定信息转换为字符
         * @param dock  绑定信息
         * @returns 字符
        */
		static String convertDockToStr(FCDockStyle dock);
	    /**
         * 单精度浮动点型数值转换为字符
         * @param value  数值
         * @returns 字符
        */
		static String convertFloatToStr(float value);
	    /**
         * 字体转换为字符
         * @param font  字体
         * @returns 字符
        */
		static String convertFontToStr(FCFont *font);
	    /**
         * 横向排列方式转换为字符
         * @param horizontalAlign  横向排列方式
         * @returns 字符
        */
		static String convertHorizontalAlignToStr(FCHorizontalAlign horizontalAlign);
	    /**
         * 整型数值转换为字符
         * @param value  数值
         * @returns 字符
        */
		static String convertIntToStr(int value);
	    /**
         * 布局样式转换为字符
         * @param layoutStyle  布局样式转
         * @returns 布局样式
        */
		static String convertLayoutStyleToStr(FCLayoutStyle layoutStyle);
	    /**
         * 长整型数值转换为字符
         * @param value   数值
         * @returns 字符串
        */
		static String convertLongToStr(Long value);
	    /**
         * 边距转换为字符
         * @param padding  边距
         * @returns 字符
        */
		static String convertPaddingToStr(const FCPadding& padding);
	    /**
         * 坐标转换为字符
         * @param point  坐标
         * @returns 字符
        */
		static String convertPointToStr(const FCPoint& mp);
	    /**
         * 矩形转换为字符
         * @param rect  矩形
         * @returns 字符
        */
		static String convertRectToStr(const FCRect& rect);
	    /**
         * 大小转换为字符
         * @param size  大小
         * @returns 字符
        */
		static String convertSizeToStr(const FCSize& size);
	    /**
         * 字符转换为锚定信息
         * @param str  字符
         * @returns 锚定信息
        */
		static FCAnchor convertStrToAnchor(const String& str);
	    /**
         * 字符转换为布尔型
         * @param str  字符
         * @returns 数值
        */
		static bool convertStrToBool(const String& str);
	    /**
         * 字符转换为颜色
         * @param str  字符
         * @returns 颜色
        */
		static Long convertStrToColor(const String& str);
	    /**
         * 字符串转内容布局
         * @param str  字符
         * @returns 字符串
        */
		static FCContentAlignment convertStrToContentAlignment(const String& str);
		/*
		* 字符串转为鼠标形状
 		*/
		static FCCursors convertStrToCursor(const String& str);
	    /**
         * 字符转换为绑定信息
         * @param str  字符
         * @returns 绑定信息
        */
		static FCDockStyle convertStrToDock(const String& str);
	    /**
         * 字符转换为双精度浮点型数值
         * @param str  字符
         * @returns 数值
        */
		static double convertStrToDouble(const String& str);
		/*
		* 字符转换为双精度浮点型数值
		*/
		static double convertStrToDouble(const wchar_t *str);
	    /**
         * 字符转换为单精度浮点型数值
         * @param str  字符
         * @returns 数值
        */
		static float convertStrToFloat(const String& str);
	    /**
         * 字符转换为字体
         * @param str  字符
         * @returns 字体
        */
		static FCFont* convertStrToFont(const String& str);
	    /**
         * 字符转换为横向排列方式
         * @param str  字符
         * @returns 字符
        */
		static FCHorizontalAlign convertStrToHorizontalAlign(const String& str);
	    /**
         * 字符转换为整型数值
         * @param str  字符
         * @returns 数值
        */
		static int convertStrToInt(const String& str);
		/**
         * 字符转换为整型数值
         * @param str  字符
         * @returns 数值
        */
		static int convertStrToInt(const wchar_t *str);
	    /**
         * 布局样式转换为字符
         * @param str  字符
         * @returns 布局样式
        */
		static FCLayoutStyle convertStrToLayoutStyle(const String& str);
	    /**
         * 字符转换位长整型数值
         * @param str  字符串
         * @returns 长整型
        */
		static Long convertStrToLong(const String& str);
		/*
		* 字符转换位长整型数值
		*/
		static Long convertStrToLong(const wchar_t *str);
	    /**
         * 字符转换为边距
         * @param str  字符
         * @returns 边距
        */
		static FCPadding convertStrToPadding(const String& str);
	    /**
         * 字符转换为坐标
         * @param str  字符
         * @returns 坐标
        */
		static FCPoint convertStrToPoint(const String& str);
	    /**
         * 字符转换为矩形
         * @param str  字符
         * @returns 矩形
        */
		static FCRect convertStrToRect(const String& str);
	    /**
         * 字符转换为大小
         * @param str  字符
         * @returns 大小
        */
		static FCSize convertStrToSize(const String& str);
	    /**
         * 字符转换为纵向排列方式
         * @param str  字符
         * @returns 字符
        */
		static FCVerticalAlign convertStrToVerticalAlign(const String& str);
	    /**
         * 纵向排列方式转换为字符
         * @param verticalAlign  横向排列方式
         * @returns 字符
        */
		static String convertVerticalAlignToStr(FCVerticalAlign verticalAlign);
		/*
		* 获取程序路径
		*/
		static string getAppPath();
		/*
		* 获取日期的数值
		*/
		static double getDateNum(int tm_year, int tm_mon, int tm_mday, int tm_hour, int tm_min, int tm_sec, int tm_msec);
		/*
		* 根据数值获取日期
		*/
        static void getDateByNum(double num, int *tm_year, int *tm_mon, int *tm_mday, int *tm_hour, int *tm_min, int *tm_sec, int *tm_msec);
	    /**
         * 获取Guid
         * @returns  Guid
        */
		static string getGuid();
		/*
		* 获取格式化后的日期
		*/
		static void getFormatDate(double date, wchar_t *str);
		/*
		* 获取格式化后的日期
		*/
		static String getFormatDate(const String& format, int year, int month, int day, int hour, int minute, int second);
	    /**
         * 根据保留小数的位置将double型转化为String型
         * @param value  值
         * @param digit 保留小数点
         * @returns  数值字符
        */
		static void getValueByDigit(double value, int digit, wchar_t *str);
		/*
		* 十六进制转十进制
		*/
		static int hexToDec(const char *str);
		/*
		* 替换字符串
		*/
		static string replace(const string& str, const string& src, const string& dest);
		/*
		* 替换字符串
		*/
		static String replace(const String& str, const String& src, const String& dest);
	    /**
         * 安全的Double转Float
         * @param value  值
         * @param digit 保留小数位数
         * @returns  Float
        */
		static float safeDoubleToFloat(double value, int digit);
		/*
		* 分割字符串
		*/
		static ArrayList<String> split(const String& str, const String& pattern);
		/*
		* 短字符串转长字符串
		*/
		static String stringTowstring(const string& strSrc);
		/*
		* 转为小写
		*/
		static String toLower(const String& str);
		/*
		* 转为大写
		*/
		static String toUpper(const String& str);
		/*
		* 字符串编码格式转换
		*/
		static void unicodeToANSC(string& out, const char* inSrc);
		/*
		* URL编码
		*/
		static string urlEncode(const std::string& szToEncode);
		/*
		* URL解码
		*/
		static string urlDecode(const std::string& szToDecode);
		/*
		* 长字符串转短字符串
		*/
		static string wstringTostring(const String& strSrc);
	};
}
#endif