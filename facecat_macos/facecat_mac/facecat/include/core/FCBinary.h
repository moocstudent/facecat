/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCBINARY_H__
#define __FCBINARY_H__
#include "stdafx.h"
#include "FCStr.h"
#pragma once
#define    DATA_LACK -1
#define PAGE_Size 8192
typedef unsigned char BYTE;
typedef BYTE *PBYTE;

namespace FaceCat{
    /**
     * 流解析
     */
    class FCBinary{
    protected:
        PBYTE m_pBase;
        unsigned int m_nDataSize;
        unsigned int m_nSize;
        unsigned int m_nInitSize;
        bool m_bSustainSize;
        static unsigned int m_dwPageSize;
        unsigned int deAllocateBuffer(unsigned int nRequestedSize);
        unsigned int getMemSize();
    public:
        /**
         * 创建流
         */
        FCBinary();
        /*
         * 析构函数
         */
        virtual ~FCBinary();
        const FCBinary& operator+(FCBinary& buff);
        bool m_bSingleRead;
        unsigned int m_nReadPos;
    public:
        /*
         * 清除
         */
        void clearBytes();
        /*
         * 拷贝
         */
        void copy(FCBinary& buffer);
        /*
         * 清除缓存
         */
        unsigned int deleteBuffer(unsigned int nSize);
        /*
         * 清除末尾
         */
        unsigned int deleteEnd(unsigned int nSize);
        /*
         * 读取文件
         */
        void fileRead(const String& strFileName);
        /*
         * 写文件
         */
        void fileWrite(const String& strFileName);
        /*
         * 获取流文件
         */
        PBYTE getBytes(unsigned int nPos = 0);
        /*
         * 获取流的长度
         */
        unsigned int getBytesLen();
        /*
         * 初始化
         */
        void initialize(unsigned int nInitsize, bool bSustain);
        /*
         * 插入
         */
        unsigned int insert(const void *pData, unsigned int nSize);
        /*
         * 插入
         */
        unsigned int insert(String& strData);
        /*
         * 读取
         */
        unsigned int read(void *pData, unsigned int nSize);
        /**
         * 读取Char数据
         */
        char readChar();
        /**
         * 读取Double数据
         */
        double readDouble();
        /**
         * 读取Float数据
         */
        float readFloat();
        /**
         * 读取Int数据
         */
        int readInt();
        /**
         * 读取Long数据
         */
        Long readLong();
        /**
         * 读取Short数据
         */
        short readShort();
        /**
         * 读取字符串数据
         */
        String readString();
        /**
         * 读取字符串数据
         */
        string readString2();
        /*
         * 重新开辟内存
         */
        unsigned int reAllocateBuffer(unsigned int nRequestedSize);
        /*
         * 跳过数据
         */
        unsigned int skipData(int nSize);
        /**
         * 写入流
         * @param bytes  流
         * @param len   长度
         */
        unsigned int write(const void *pData, unsigned int nSize);
        /*
         * 写入String型数据
         */
        unsigned int write(String& strData);
        /**
         * 写入Char型数据
         * @param val   Char型数据
         */
        void writeChar(char cValue);
        /**
         * 写入Double型数据
         * @param val   Double型数据
         */
        void writeDouble(double dValue);
        /**
         * 写入Float型数据
         * @param val   Float型数据
         */
        void writeFloat(float fValue);
        /**
         * 写入Int型数据
         * @param val   Int型数据
         */
        void writeInt(int nValue);
        /**
         * 写入Long型数据
         * @param val   Long型数据
         */
        void writeLong(Long hValue);
        /**
         * 写入Short型数据
         * @param val   Short型数据
         */
        void writeShort(short sValue);
        /**
         * 写入字符串数据
         * @param val   字符串数据
         */
        unsigned int writeString(const String& strData);
        /**
         * 写入字符串数据
         * @param val   字符串数据
         */
        unsigned int writeString(const string& strData);
    };
}

#endif
