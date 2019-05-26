/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FaceCat {
    /// <summary>
    /// 流解析
    /// </summary>
    public class FCBinary {
        /// <summary>
        /// 创建流
        /// </summary>
        public FCBinary() {
            m_outputStream = new MemoryStream();
            m_writer = new BinaryWriter(m_outputStream, Encoding.UTF8);
        }

        /// <summary>
        /// 输入流
        /// </summary>
        private MemoryStream m_inputStream;

        /// <summary>
        /// 输出流
        /// </summary>
        private MemoryStream m_outputStream;

        /// <summary>
        /// 读取器
        /// </summary>
        private BinaryReader m_reader;

        /// <summary>
        /// 写入器
        /// </summary>
        private BinaryWriter m_writer;

        /// <summary>
        /// 关闭
        /// </summary>
        public void close() {
            try {
                if (m_reader != null) {
                    m_reader.Close();
                }
                if (m_writer != null) {
                    m_writer.Close();
                }
                if (m_inputStream != null) {
                    m_inputStream.Close();
                    m_inputStream.Dispose();
                }
                if (m_outputStream != null) {
                    m_outputStream.Close();
                    m_outputStream.Dispose();
                }
            }
            catch (Exception ex) {

            }
        }

        /// <summary>
        /// 获取Bytes型数据
        /// </summary>
        /// <returns>Bytes型数据</returns>
        public byte[] getBytes() {
            return m_outputStream.ToArray();
        }

        /// <summary>
        /// 获取布尔型数据
        /// </summary>
        /// <returns>布尔型数据</returns>
        public bool readBool() {
            return m_reader.ReadBoolean();
        }

        /// <summary>
        /// 获取Byte型数据
        /// </summary>
        /// <returns>Byte型数据</returns>
        public byte readByte() {
            return m_reader.ReadByte();
        }

        /// <summary>
        /// 读取流数据
        /// </summary>
        /// <param name="bytes">流数据</param>
        public void readBytes(byte[] bytes) {
            int bytesSize = bytes.Length;
            for (int i = 0; i < bytesSize; i++) {
                bytes[i] = m_reader.ReadByte();
            }
        }

        /// <summary>
        /// 读取Char数据
        /// </summary>
        /// <returns>Char数据</returns>
        public char readChar() {
            return m_reader.ReadChar();
        }

        /// <summary>
        /// 读取Double数据
        /// </summary>
        /// <returns>Double数据</returns>
        public double readDouble() {
            return m_reader.ReadDouble();
        }

        /// <summary>
        /// 读取Float数据
        /// </summary>
        /// <returns>Float数据</returns>
        public float readFloat() {
            return m_reader.ReadSingle();
        }

        /// <summary>
        /// 读取Int数据
        /// </summary>
        /// <returns>Int数据</returns>
        public int readInt() {
            return m_reader.ReadInt32();
        }

        /// <summary>
        /// 读取Short数据
        /// </summary>
        /// <returns>Short数据</returns>
        public short readShort() {
            return m_reader.ReadInt16();
        }

        /// <summary>
        /// 读取字符串数据
        /// </summary>
        /// <returns>字符串数据</returns>
        public String readString() {
            int size = m_reader.ReadInt32();
            byte[] bytes = m_reader.ReadBytes(size);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 写入流
        /// </summary>
        /// <param name="bytes">流</param>
        /// <param name="len">长度</param>
        public void write(byte[] bytes, int len) {
            m_inputStream = new MemoryStream(bytes);
            m_reader = new BinaryReader(m_inputStream, Encoding.UTF8);
        }

        /// <summary>
        /// 写入Bool型数据
        /// </summary>
        /// <param name="val">Bool型数据</param>
        public void writeBool(bool val) {
            m_writer.Write(val);
        }

        /// <summary>
        /// 写入Byte型数据
        /// </summary>
        /// <param name="val">Byte型数据</param>
        public void writeByte(byte val) {
            m_writer.Write(val);
        }

        /// <summary>
        /// 写入Bytes数据
        /// </summary>
        /// <param name="val">Bytes数据</param>
        public void writeBytes(byte[] val) {
            m_writer.Write(val);
        }

        /// <summary>
        /// 写入Char型数据
        /// </summary>
        /// <param name="val">Char型数据</param>
        public void writeChar(char val) {
            m_writer.Write(val);
        }

        /// <summary>
        /// 写入Double型数据
        /// </summary>
        /// <param name="val">Double型数据</param>
        public void writeDouble(double val) {
            m_writer.Write(val);
        }

        /// <summary>
        /// 写入Float型数据
        /// </summary>
        /// <param name="val">Float型数据</param>
        public void writeFloat(float val) {
            m_writer.Write(val);
        }

        /// <summary>
        /// 写入Int型数据
        /// </summary>
        /// <param name="val">Int型数据</param>
        public void writeInt(int val) {
            m_writer.Write(val);
        }

        /// <summary>
        /// 写入Short型数据
        /// </summary>
        /// <param name="val">Short型数据</param>
        public void writeShort(short val) {
            m_writer.Write(val);
        }

        /// <summary>
        /// 写入字符串数据
        /// </summary>
        /// <param name="val">字符串数据</param>
        public void writeString(String val) {
            byte[] bytes = Encoding.UTF8.GetBytes(val);
            m_writer.Write(bytes.Length);
            m_writer.Write(bytes);
        }
    }
}
