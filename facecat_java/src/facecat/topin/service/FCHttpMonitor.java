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
import facecat.topin.chart.*;
import java.io.*;
import java.net.*;
import java.util.concurrent.*;
import java.util.*;

/**
 * Node服务
 *
 */
public class FCHttpMonitor {

    class SocketHandler implements Runnable {

        @Override
        public void run() {
            readData(m_socket);
        }
        public Socket m_socket;
    }

    /**
     * 创建Node服务
     *
     * @param fileName 文件
     */
    public FCHttpMonitor(String fileName) {
        m_fileName = fileName;
    }

    /**
     * 文件名称
     *
     */
    private String m_fileName;

    /*
     Http数据
     */
    public java.util.HashMap<Integer, FCHttpData> m_httpDatas = new java.util.HashMap<Integer, FCHttpData>();

    /*
     脚本集合
     */
    private java.util.Stack<FCScript> m_indicators = new java.util.Stack<FCScript>();

    public static java.util.HashMap<String, FCHttpEasyService> m_easyServices = new java.util.HashMap<String, FCHttpEasyService>();

    private FCScript m_indicator;

    /**
     * 获取主脚
     *
     */
    public final FCScript getIndicator() {
        return m_indicator;
    }

    private ServerSocket m_listener;

    /**
     * 获取监听对象
     *
     */
    public final ServerSocket getListener() {
        return m_listener;
    }

    private static FCHttpMonitor m_mainMonitor;

    public static FCHttpMonitor getMainMonitor() {
        return m_mainMonitor;
    }

    private int m_port = 8080;

    /**
     * 获取或设置端口号 *
     */
    public final int getPort() {
        return m_port;
    }

    public final void setPort(int port) {
        m_port = port;
    }

    private String m_script;

    /**
     * 获取脚本
     *
     */
    public final String getScript() {
        return m_script;
    }

    private FCNative m_native;

    /**
     * 获取或设置XML对象
     *
     */
    public final FCNative getNative() {
        return m_native;
    }

    private boolean m_useScript;

    /**
     * 获取是否使用脚本
     */
    public final boolean useScript() {
        return m_useScript;
    }

    /**
     * 检查脚�?
     *
     */
    public final void checkScript() {
        String newScript = "";
        RefObject<String> tempRef_newScript = new RefObject<String>(newScript);
        FCFile.read(m_fileName, tempRef_newScript);
        newScript = tempRef_newScript.argvalue;
        if (!m_script.equals(newScript)) {
            System.out.println("检测到脚本被修改.....");
            m_script = newScript;
            synchronized (m_indicators) {
                while (true) {
                    try {
                        FCScript indicator = m_indicators.pop();
                        indicator.clear();
                    } catch (java.lang.Exception e) {
                        break;
                    }
                }
            }
        }
    }

    /*
     读取数据
     */
    private static void readData(Socket socket) {
        FCHttpMonitor nodeService = m_mainMonitor;
        int newSocketID = socket.hashCode();
        try {
            DataInputStream sReader = new DataInputStream(socket.getInputStream());
            byte[] recvDatas = new byte[102400];
            int len = sReader.read(recvDatas);
            ByteArrayInputStream byteArray = new ByteArrayInputStream(recvDatas);
            BufferedReader reader = new BufferedReader(new InputStreamReader(byteArray));
            FCHttpData data = new FCHttpData();
            String requestHeader;
            data.m_remoteIP = socket.getInetAddress().getHostAddress();
            data.m_remotePort = socket.getPort();

            int contentLength = 0;
            String parameters = "";
            while ((requestHeader = reader.readLine()) != null && !requestHeader.isEmpty()) {
                String lowerHeader = requestHeader.toLowerCase();
                if (lowerHeader.indexOf("get") == 0) {
                    int end = lowerHeader.indexOf("http/");
                    data.m_method = "GET";
                    parameters = requestHeader.substring(5, end - 1);
                } else if (lowerHeader.indexOf("post") == 0) {
                    int end = lowerHeader.indexOf("http/");
                    data.m_method = "POST";
                    parameters = requestHeader.substring(5, end - 1);
                } else if (lowerHeader.indexOf("accept: ") == 0) {
                    try {
                        data.m_contentType = requestHeader.substring(8, requestHeader.indexOf(','));
                    } catch (Exception ex) {
                    }
                } else if (lowerHeader.indexOf("content-type:") == 0) {
                    data.m_contentType = requestHeader.substring(14);
                } else if (lowerHeader.indexOf("host:") == 0) {
                    data.m_url = requestHeader.substring(requestHeader.indexOf(':') + 2);
                } else if (lowerHeader.indexOf("content-length") == 0) {
                    int begin = lowerHeader.indexOf("content-length:") + "content-length:".length();
                    String postParamterLength = requestHeader.substring(begin).trim();
                    contentLength = Integer.parseInt(postParamterLength);
                }
            }
            if (contentLength > 0) {
                data.m_body = new byte[contentLength];
                int idx = 0, ide = 0;
                while (idx < contentLength) {
                    int recvData = reader.read();
                    if (recvData != -1) {
                        if (recvData != 0) {
                            ide++;
                        }
                        idx++;
                    } else {
                        break;
                    }
                }
                reader.close();
                byteArray.close();
                if (ide == 0) {
                    sReader.read(data.m_body);
                } else {
                    for (int i = 0; i < contentLength; i++) {
                        data.m_body[i] = recvDatas[len - contentLength + i];
                    }
                }
                data.m_contentLength = contentLength;
            } else {
                reader.close();
                byteArray.close();
            }
            if (data.m_method.length() == 0) {
                sReader.close();
                return;
            }
            int cindex = parameters.indexOf('?');
            if (cindex != -1) {
                data.m_url = data.m_url + "/" + parameters;
                parameters = parameters.substring(cindex + 1);
                String[] strs = parameters.split("[&]");
                int strsSize = strs.length;
                String text = "";
                for (int i = 0; i < strsSize; i++) {
                    String[] subStrs = strs[i].split("[=]");
                    data.m_parameters.put(subStrs[0].toLowerCase(), subStrs[1]);
                }
            } else {
                data.m_url += "/" + parameters;
            }
            FCScript indicator = null;
            if (nodeService.useScript()) {
                try {
                    synchronized (nodeService.m_indicators) {
                        indicator = nodeService.m_indicators.pop();
                    }
                } catch (java.lang.Exception e) {
                    indicator = CFunctionEx.createIndicator(nodeService.m_script, nodeService.m_native);
                }
                ArrayList<CFunction> functions = indicator.getFunctions();
                int functionsSize = functions.size();
                for (int i = 0; i < functionsSize; i++) {
                    CFunctionHttp function = (CFunctionHttp) ((functions.get(i) instanceof CFunctionHttp) ? functions.get(i) : null);
                    if (function != null) {
                        function.m_data = data;
                    }
                }
            }
            data.m_socketID = newSocketID;
            synchronized (nodeService.m_httpDatas) {
                nodeService.m_httpDatas.put(newSocketID, data);
            }
            if (indicator != null) {
                double value = indicator.callFunction("ONHTTPREQUEST();");
            }
            if (data.m_close) {
                sReader.close();;
                return;
            }
            int resContentLength = 0;
            if (data.m_resBytes != null) {
                resContentLength = data.m_resBytes.length;
            } else {
                resContentLength = data.m_resStr.getBytes().length;
            }
            StringBuilder bld = new StringBuilder();
            bld.append("HTTP/1.0 " + FCStr.convertIntToStr(data.m_statusCode) + " OK\r\n");
            bld.append(String.format("Content-Length: %d\r\n", resContentLength));
            bld.append("Connection: close\r\n\r\n");
            if (data.m_resBytes != null) {
                DataOutputStream writer = new DataOutputStream(socket.getOutputStream());
                writer.write(bld.toString().getBytes());
                writer.write(data.m_resBytes);
                writer.flush();
                writer.close();
            } else {
                BufferedWriter writer = new BufferedWriter(new OutputStreamWriter(socket.getOutputStream(), "GB2312"));
                writer.write(bld.toString());
                writer.write(data.m_resStr);
                writer.flush();
                writer.close();
            }
            if (indicator != null) {
                synchronized (nodeService.m_indicators) {
                    nodeService.m_indicators.push(indicator);
                }
            }
            sReader.close();
        } catch (Exception ex) {
            ex.printStackTrace();
        } finally {
            synchronized (nodeService.m_httpDatas) {
                nodeService.m_httpDatas.remove(newSocketID);
            }
            try {
                socket.close();
            } catch (Exception ex) {

            }
        }
    }

    /**
     * 启动监听
     *
     */
    public int start() {
        m_useScript = FCFile.isFileExist(m_fileName);
        if (m_useScript) {
            m_native = new FCNative();
            RefObject<String> tempRef_m_script = new RefObject<String>(m_script);
            FCFile.read(m_fileName, tempRef_m_script);
            m_script = tempRef_m_script.argvalue;
            m_indicator = CFunctionEx.createIndicator(m_script, m_native);
        }
        try {
            if (m_indicator != null) {
                m_indicator.callFunction("ONHTTPSERVERSTARTING('" + m_fileName + "');");
            }
            m_listener = new ServerSocket(m_port);
            if (m_indicator != null) {
                m_indicator.callFunction("ONHTTPSERVERSTART(0,0);");
            }
            ExecutorService service = Executors.newCachedThreadPool();
            boolean appAlive = false;
            while (appAlive) {
                Socket socket = m_listener.accept();
                SocketHandler receiver = new SocketHandler();
                receiver.m_socket = socket;
                service.execute(receiver);
            }
            service.shutdown();
            service.awaitTermination(10, TimeUnit.SECONDS);
            return 1;
        } catch (Exception ex) {
            if (m_indicator != null) {
                m_indicator.callFunction("ONHTTPSERVERSTARTFAIL('" + ex.getMessage() + "');");
            }
        }
        return -1;
    }
}
