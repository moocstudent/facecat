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

/**
 * 流解析
 */
public class FCBinary {

    /**
     * 创建流
     */
    public FCBinary() {
        m_outputStream = new ByteArrayOutputStream();
        m_writer = new DataOutputStream(m_outputStream);
    }

    /**
     * 输入流
     */
    private ByteArrayInputStream m_inputStream;

    /**
     * 输出流
     */
    private ByteArrayOutputStream m_outputStream;

    /**
     * 读取器
     */
    private DataInputStream m_reader;

    /**
     * 写入器
     */
    private DataOutputStream m_writer;

    /**
     * 关闭
     */
    public void close() throws IOException {
        if (m_reader != null) {
            m_reader.close();
        }
        if (m_writer != null) {
            m_writer.close();;
        }
        if (m_inputStream != null) {
            m_inputStream.close();
        }
        if (m_outputStream != null) {
            m_outputStream.close();
        }
    }

    /**
     * 获取Bytes型数据
     */
    public byte[] getBytes() {
        return m_outputStream.toByteArray();
    }

    /**
     * 获取布尔型数据
     */
    public boolean readBool() throws IOException {
        return m_reader.readBoolean();
    }

    /**
     * 获取Byte型数据
     */
    public byte readByte() throws IOException {
        return m_reader.readByte();
    }

    /**
     * 读取流数据
     */
    public void readBytes(byte[] bytes) throws IOException {
        m_reader.read(bytes);
    }

    /**
     * 读取Char数据
     */
    public char readChar() throws IOException {
        return (char) m_reader.readByte();
    }

    /**
     * 读取Double数据
     */
    public double readDouble() throws IOException {
        byte[] buffer = new byte[8];
        m_reader.read(buffer);
        long l = (0xffL & (long) buffer[0]) | (0xff00L & ((long) buffer[1] << 8)) | (0xff0000L & ((long) buffer[2] << 16))
                | (0xff000000L & ((long) buffer[3] << 24))
                | (0xff00000000L & ((long) buffer[4] << 32)) | (0xff0000000000L & ((long) buffer[5] << 40))
                | (0xff000000000000L & ((long) buffer[6] << 48)) | (0xff00000000000000L & ((long) buffer[7] << 56));
        return Double.longBitsToDouble(l);
    }

    /**
     * 读取Float数据
     */
    public float readFloat() throws IOException {
        return Float.intBitsToFloat(readInt());
    }

    /**
     * 读取Int数据
     */
    public int readInt() throws IOException {
        byte[] buffer = new byte[4];
        m_reader.read(buffer);
        return (int) ((buffer[0] & 0xFF)
                | ((buffer[1] & 0xFF) << 8)
                | ((buffer[2] & 0xFF) << 16)
                | ((buffer[3] & 0xFF) << 24));
    }

    /**
     * 读取Short数据
     */
    public short readShort() throws IOException {
        byte[] buffer = new byte[2];
        m_reader.read(buffer);
        return (short) ((0xff & buffer[0]) | (0xff00 & (buffer[1] << 8)));
    }

    /**
     * 读取字符串数据
     */
    public String readString() throws IOException {
        String str = "";
        int strSize = readInt();
        byte[] bytes = new byte[strSize];
        m_reader.read(bytes);
        if (strSize >= 1) {
            str = new String(bytes, "UTF-8");
        }
        return str;
    }

    /**
     * 写入流
     *
     * @param bytes 流
     * @param len 长度
     */
    public void write(byte[] bytes, int len) {
        m_inputStream = new ByteArrayInputStream(bytes);
        m_reader = new DataInputStream(m_inputStream);
    }

    /**
     * 写入Bool型数据
     *
     * @param val Bool型数据
     */
    public void writeBool(boolean val) throws IOException {
        m_writer.writeBoolean(val);
    }

    /**
     * 写入Byte型数据
     *
     * @param val Byte型数据
     */
    public void writeByte(byte val) throws IOException {
        m_writer.writeByte(val);
    }

    /**
     * 写入Bytes数据
     *
     * @param val Bytes数据
     */
    public void writeBytes(byte[] val) throws IOException {
        m_writer.write(val);
    }

    /**
     * 写入Char型数据
     *
     * @param val Char型数据
     */
    public void writeChar(char val) throws IOException {
        m_writer.writeByte(val);
    }

    /**
     * 写入Double型数据
     *
     * @param val Double型数据
     */
    public void writeDouble(double val) throws IOException {
        long intBits = Double.doubleToLongBits(val);
        byte[] buffer = new byte[8];
        buffer[0] = (byte) (intBits & 0xff);
        buffer[1] = (byte) ((intBits >> 8) & 0xff);
        buffer[2] = (byte) ((intBits >> 16) & 0xff);
        buffer[3] = (byte) ((intBits >> 24) & 0xff);
        buffer[4] = (byte) ((intBits >> 32) & 0xff);
        buffer[5] = (byte) ((intBits >> 40) & 0xff);
        buffer[6] = (byte) ((intBits >> 48) & 0xff);
        buffer[7] = (byte) ((intBits >> 56) & 0xff);
        m_writer.write(buffer);
    }

    /**
     * 写入Float型数据
     *
     * @param val Float型数据
     */
    public void writeFloat(float val) throws IOException {
        int intBits = Float.floatToIntBits(val);
        byte[] buffer = new byte[4];
        buffer[0] = (byte) (intBits & 0xff);
        buffer[1] = (byte) ((intBits >> 8) & 0xff);
        buffer[2] = (byte) ((intBits >> 16) & 0xff);
        buffer[3] = (byte) (intBits >> 24);
        m_writer.write(buffer);
    }

    /**
     * 写入Int型数据
     *
     * @param val Int型数据
     */
    public void writeInt(int val) throws IOException {
        byte[] buffer = new byte[4];
        buffer[0] = (byte) (val & 0xff);
        buffer[1] = (byte) ((val & 0xff00) >> 8);
        buffer[2] = (byte) ((val & 0xff0000) >> 16);
        buffer[3] = (byte) ((val & 0xff000000) >> 24);
        m_writer.write(buffer);
    }

    /**
     * 写入Short型数据
     *
     * @param val Short型数据
     */
    public void writeShort(short val) throws IOException {
        byte[] buffer = new byte[2];
        buffer[0] = (byte) (val & 0xff);
        buffer[1] = (byte) ((val & 0xff00) >> 8);
        m_writer.write(buffer);
    }

    /**
     * 写入字符串数据
     *
     * @param val 字符串数据
     */
    public void writeString(String val) throws IOException {
        if (val == null || val.length() == 0) {
            writeInt(0);
        } else {
            byte[] bytes = val.getBytes("UTF-8");
            int len = bytes.length;
            writeInt(len);
            m_writer.write(bytes);
        }
    }
}
