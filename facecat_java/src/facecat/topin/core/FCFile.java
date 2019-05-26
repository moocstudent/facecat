/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.core;

import java.io.*;
import java.util.*;

/**
 * 文件操作类
 *
 */
public class FCFile {

    public static final int OF_READWRITE = 2;
    public static final int OF_SHARE_DENY_NONE = 0x40;

    /**
     * 向文件中追加内容
     *
     * @param file 文件
     * @param content 内容
     * @return 是否成功
     */
    public static boolean append(String file, String content) {
        try {
            OutputStream os = new FileOutputStream(new File(file), true);
            os.write(content.getBytes());
            os.close();
            return true;
        } catch (Exception e) {
            return false;
        }
    }

    /**
     * 创建文件夹
     *
     * @param dir 文件夹
     */
    public static void createDirectory(String dir) {
        try {
            File f = new File(dir);
            if (f.exists()) {
                f.mkdir();
            }
        } catch (Exception ex) {
        }
    }

    /**
     * 获取文件夹中的文件夹
     *
     * @param dir 文件夹
     * @param dirs 文件夹集合
     * @return
     */
    public static boolean getDirectories(String dir, ArrayList<String> dirs) {
        try {
            File f = new File(dir);
            if (f.exists() && f.isDirectory()) {
                File[] fList = f.listFiles();
                int fListSize = fList.length;
                for (int i = 0; i < fListSize; i++) {
                    File subFile = fList[i];
                    if (subFile.isDirectory()) {
                        dirs.add(subFile.getAbsolutePath());
                    }
                }
                return true;
            }
            return false;
        } catch (Exception ex) {
            return false;
        }
    }

    /**
     * 获取文件夹中的文件
     *
     * @param dir 文件夹
     * @param files 文件集合
     * @return 是否成功
     */
    public static boolean getFiles(String dir, ArrayList<String> files) {
        try {
            File f = new File(dir);
            if (f.exists() && f.isDirectory()) {
                File[] fList = f.listFiles();
                int fListSize = fList.length;
                for (int i = 0; i < fListSize; i++) {
                    File subFile = fList[i];
                    if (subFile.isFile()) {
                        files.add(subFile.getAbsolutePath());
                    }
                }
                return true;
            }
            return false;
        } catch (Exception ex) {
            return false;
        }
    }

    /**
     * 判断文件夹是否存在
     *
     * @param dir 文件夹
     * @return 是否存在
     */
    public static boolean isDirectoryExist(String dir) {
        try {
            File f = new File(dir);
            return f.isDirectory() && f.exists();
        } catch (Exception ex) {
            return false;
        }
    }

    /**
     * 判断文件是否存在
     *
     * @param file 文件
     * @return 是否存在
     */
    public static boolean isFileExist(String file) {
        try {
            File f = new File(file);
            return f.isFile() && f.exists();
        } catch (Exception ex) {
            return false;
        }
    }

    /**
     * 从文件中读取内容
     *
     * @param file 文件
     * @param content 返回内容
     * @return 是否成功
     */
    public static boolean read(String file, RefObject<String> content) {
        try {
            InputStream is = new FileInputStream(new File(file));
            byte[] buffer = new byte[is.available()];
            // 新建一个字节数组
            is.read(buffer);
            content.argvalue = new String(buffer);
            is.close();
            return true;
        } catch (Exception e) {
            return false;
        }
    }

    /**
     * 移除文件
     *
     * @param file 文件
     * @return 是否成功
     */
    public static boolean removeFile(String file) {
        return false;
    }

    /**
     * 向文件中写入内容
     *
     * @param file 文件
     * @param content 内容
     * @return 是否成功
     */
    public static boolean write(String file, String content) {
        try {
            OutputStream os = new FileOutputStream(new File(file));
            os.write(content.getBytes());
            os.close();
            return true;
        } catch (Exception e) {
            return false;
        }
    }
}
