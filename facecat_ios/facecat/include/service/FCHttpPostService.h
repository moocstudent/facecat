/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu)
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

#ifndef __FCHTTPPOSTSERVICE_H__
#define __FCHTTPPOSTSERVICE_H__
#pragma once
#include "stdafx.h"
#include "FCClientService.h"
#include "FCFile.h"

#define SERVICEID_HTTPPOST 20
#define FUNCTIONID_HTTPPOST_TEST 0

namespace FaceCat{
    /*
     * Post服务
     */
    class FCHttpPostService : public FCClientService{
    private:
        /**
         * 是否同步发送
         */
        bool m_isSyncSend;
        /*
         * 锁
         */
        FCLock m_lock;
        /**
         * 地址
         */
        string m_url;
        /**
         * Timeout时间
         */
        int m_timeout;
    public:
        vector<FCMessage*> m_messages;
        /**
         * 创建HTTP服务
         */
        FCHttpPostService();
        /*
         * 析构函数
         */
        virtual ~FCHttpPostService();
        /**
         * 获取是否同步发送
         */
        bool isSyncSend();
        /**
         * 设置是否同步发送
         */
        void setIsSyncSend(bool isSyncSend);
        /**
         * 获取Timeout时间
         */
        int getTimeout();
        /**
         * 设置Timeout时间
         */
        void setTimeout(int timeout);
        /**
         * 获取地址
         */
        string getUrl();
        /**
         * 设置地址
         */
        void setUrl(string url);
    public:
        /*
         * 获取锁
         */
        FCLock getLock();
        /**
         * 接收数据
         * @param  message  消息
         */
        virtual void onReceive(FCMessage *message);
        /**
         * 发送POST数据
         * @param  url  地址
         * @param  data  数据
         */
        String post(const String& url);
        /*
         * 发送POST数据
         */
        string post(const String& url, char *sendDatas, int sendSize);
        /*
         * 发送POST数据
         */
        String post(const String& url, const String &data);        /*
                                                                    * 发送数据
                                                                    */
        int send(FCMessage *message);
        /**
         * 发送POST数据
         * @param  message  消息
         */
        int sendRequest(FCMessage *message);
    };
}

#endif
