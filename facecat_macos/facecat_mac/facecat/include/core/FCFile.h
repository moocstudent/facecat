/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCFILE_H__
#define __FCFILE_H__
#pragma once
#include "stdafx.h"
#include <fstream>
#include <sys/stat.h>
#include <dirent.h>
#include <pwd.h>

namespace FaceCat{
    /*
     * 文件处理
     */
    class FCFile
    {
    public:
        /**
         * 追加内容
         */
        static bool append(const char *file, const char *content);
        /**
         * 追加内容-宽字符
         */
        static bool append(const String &file, const String &content);
        /*
         * 拷贝文件
         */
        void copyFileLinux(const string &srcDirPath, const string &desDirPath);
        /**
         * 创建目录
         */
        static void createDirectory(const char *dir);
        /**
         * 创建目录-宽字符
         */
        static void createDirectory(const String &dir);
        /**
         * 判断目录是否存在
         */
        static bool isDirectoryExist(const char *dir);
        /**
         * 判断目录是否存在-宽字符
         */
        static bool isDirectoryExist(const String &dir);
        /**
         * 文件是否存在
         */
        static bool isFileExist(const char *file);
        /**
         * 文件是否存在-宽字符
         */
        static bool isFileExist(const String &file);
        /**
         * 获取目录
         */
        static bool getDirectories(const char *dir, ArrayList<string> *dirs);
        /**
         * 获取目录-宽字符
         */
        static bool getDirectories(const String &file, ArrayList<String> *dirs);
        /**
         * 获取文件长度
         */
        static int getFileLength(const char *file);
        /**
         * 获取文件长度-宽字符
         */
        static int getFileLength(const String &file);
        /**
         * 获取文件
         */
        static bool getFiles(const char *dir, ArrayList<string> *files);
        /**
         * 获取文件-宽字符
         */
        static bool getFiles(const String &dir, ArrayList<String> *files);
        /**
         * 获取文件状态
         */
        static int getFileState(const char *file, struct stat *buf);
        /**
         * 获取文件状态-宽字符
         */
        static int getFileState(const String &file, struct stat *buf);
        /**
         * 读取文件
         */
        static bool read(const char *file, string *content);
        /**
         * 读取文件-宽字符
         */
        static bool read(const String &file, String *content);
        /**
         * 移除文件
         */
        static void removeFile(const char *file);
        /**
         * 移除文件-宽字符
         */
        static void removeFile(const String &file);
        /**
         * 写入文件
         */
        static bool write(const char *file, const char *content);
        /**
         * 写入文件-宽字符
         */
        static bool write(const String &file, const String &content);
    private:
        /*
         * 创建文件夹
         */
        static bool makeDir (const string& pathName);
        /*
         * 获取文件名称
         */
        static bool getFileName(string srcDirPath , ArrayList<string> *fileNameList);
        /*
         * 拷贝文件
         */
        static void doCopy(string srcDirPath , string desDirPath, ArrayList<string> *fileNameLis);
    };
}
#endif
