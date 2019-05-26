/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.service;

import facecat.topin.core.*;
import java.io.*;
import java.net.*;
import java.util.*;

/*
 HTTP流服务
 */
public class FCHttpPostService extends FCServerService {
    /*
     * 创建服务
     */

    public FCHttpPostService() {
    }

    public static ArrayList<FCMessage> m_messages = new ArrayList<>();

    private boolean m_isSyncSend;

    public boolean isSyncSend() {
        return m_isSyncSend;
    }

    public void setIsSyncSend(boolean isSyncSend) {
        m_isSyncSend = isSyncSend;
    }

    private int m_timeout = 10;

    public int getTimeout() {
        return m_timeout;
    }

    public void setTimeout(int timeout) {
        m_timeout = timeout;
    }

    private String m_url;

    public String getUrl() {
        return m_url;
    }

    public void setUrl(String value) {
        m_url = value;
    }

    @Override
    public void onReceive(FCMessage message) {
        super.onReceive(message);
        sendToListener(message);
    }

    /*
     * 发送POST数据
     */
    public String post(String url) {
        return post(url, "");
    }

    /*
     * 发送POST数据
     */
    public String post(String url, String data) {
        byte[] sendDatas = data.getBytes();
        byte[] bytes = post(url, sendDatas);
        if (bytes != null) {
            return bytes.toString();
        } else {
            return null;
        }
    }

    /*
     * 发送POST数据
     */
    public byte[] post(String url, byte[] sendDatas) {
        try {
            URL realUrl = new URL(url);
            URLConnection conn = realUrl.openConnection();
            conn.setRequestProperty("accept", "text/html");
            conn.setRequestProperty("connection", "Keep-Alive");
            conn.setRequestProperty("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1;SV1)");
            conn.setDoOutput(true);
            conn.setDoInput(true);
            if (sendDatas != null) {
                DataOutputStream writer = new DataOutputStream(conn.getOutputStream());
                writer.write(sendDatas);
                writer.flush();
                writer.close();
            }
            DataInputStream reader = new DataInputStream(conn.getInputStream());
            FCBinary br = new FCBinary();
            while (true) {
                int data = reader.read();
                if (data != -1) {
                    br.writeByte((byte) data);
                } else {
                    break;
                }
            }
            reader.close();
            byte[] dataArray = br.getBytes();
            br.close();
            return dataArray;
        } catch (Exception ex) {
            return null;
        }
    }

    @Override
    public int send(FCMessage message) {

        synchronized (m_messages) {
            m_messages.add(message);
        }
        if (!m_isSyncSend) {
            new Thread(new Runnable() {
                @Override
                public void run() {
                    FCMessage message = null;
                    synchronized (m_messages) {
                        message = m_messages.get(0);
                    }
                    if (message == null) {
                        return;
                    }
                    sendRequest(message);
                }
            }).start();

            return 1;
        } else {
            return sendRequest(message);
        }
    }

    /**
     * 发送请求
     */
    public int sendRequest(FCMessage message) {

        try {
            int ret = -1;
            FCBinary bw = new FCBinary();
            byte[] body = message.m_body;
            int bodyLength = message.m_bodyLength;
            int uncBodyLength = bodyLength;
            byte[] zipBody = null;
            if (message.m_compressType == COMPRESSTYPE_GZIP) {
                zipBody = FCStr.gZip(body);
                bodyLength = zipBody.length;
            }
            int len = 4 * 4 + bodyLength + 2 * 3 + 1 * 2;
            bw.writeInt(len);
            bw.writeShort((short) message.m_groupID);
            bw.writeShort((short) message.m_serviceID);
            bw.writeShort((short) message.m_functionID);
            bw.writeInt(message.m_sessionID);
            bw.writeInt(message.m_requestID);
            bw.writeChar((char) message.m_state);
            bw.writeChar((char) message.m_compressType);
            bw.writeInt(uncBodyLength);
            if (message.m_compressType == COMPRESSTYPE_GZIP) {
                bw.writeBytes(zipBody);
            } else {
                bw.writeBytes(body);
            }
            byte[] bytes = bw.getBytes();
            URL realUrl = new URL(m_url);
            URLConnection conn = realUrl.openConnection();
            conn.setRequestProperty("accept", "text/html");
            conn.setRequestProperty("connection", "Keep-Alive");
            conn.setRequestProperty("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1;SV1)");
            conn.setDoOutput(true);
            conn.setDoInput(true);
            if (bytes != null) {
                DataOutputStream writer = new DataOutputStream(conn.getOutputStream());
                writer.write(bytes);
                writer.flush();
                writer.close();
            }
            DataInputStream reader = new DataInputStream(conn.getInputStream());
            FCBinary br = new FCBinary();
            while (true) {
                int data = reader.read();
                if (data != -1) {
                    br.writeByte((byte) data);
                } else {
                    break;
                }
            }
            byte[] dataArray = br.getBytes();
            if (dataArray != null) {
                ret = dataArray.length;
            }
            m_upFlow += ret;
            FCServerService.callBack(message.m_socketID, 0, dataArray, ret);
            br.close();
            bw.close();
            reader.close();
            return ret;
        } catch (Exception ex) {
            return 0;
        }
    }
}
