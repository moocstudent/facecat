/*基于捂脸猫FaceCat框架 v1.0
 捂脸猫创始人-矿洞程序员-脉脉KOL-陶德 (微信号:suade1984);
 */
using System;
using System.Collections.Generic;
using System.Text;
using FaceCat;
using System.Windows.Forms;

namespace FaceCat
{
    public class DesignerScript :FCUIScript
    {
        /// <summary>
        /// 创建脚本
        /// </summary>
        /// <param name="xml">XML对象</param>
        public DesignerScript(FCUIXml xml)
        {
            m_xml = xml;
        }

        /// <summary>
        /// 脚本对象
        /// </summary>
        private FCScript m_script;

        /// <summary>
        /// 脚本文本
        /// </summary>
        private String m_text;

        private bool m_deleted = false;

        /// <summary>
        /// 获取是否被销毁
        /// </summary>
        public bool IsDeleted
        {
            get { return m_deleted; }
        }

        private FCUIXml m_xml;

        /// <summary>
        /// 获取或设置XML对象
        /// </summary>
        public FCUIXml Xml
        {
            get { return m_xml; }
            set { m_xml = value; }
        }

        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="function">方法文本</param>
        /// <returns>返回值</returns>
        public String callFunction(String function)
        {
            return m_script.callFunction(function).ToString();
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public virtual void delete()
        {
            if (!m_deleted)
            {
                if (m_script != null)
                {
                    m_script.delete();
                }
                m_deleted = true;
            }
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性值</returns>
        public String getProperty(String name, String propertyName)
        {
            if (m_xml != null)
            {
                FCView control = m_xml.findControl(name);
                if (control != null)
                {
                    String value = null, type = null;
                    control.getProperty(propertyName, ref value, ref type);
                    return value;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取调用者
        /// </summary>
        /// <returns>调用者名称</returns>
        public String getSender()
        {
            if (m_xml != null)
            {
                FCUIEvent uiEvent = m_xml.Event;
                if (uiEvent != null)
                {
                    return uiEvent.Sender;
                }
            }
            return null;
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="name">控件ID</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="propertyValue">属性值</param>
        public void setProperty(String name, String propertyName, String propertyValue)
        {
            if (m_xml != null)
            {
                FCView control = m_xml.findControl(name);
                if (control != null)
                {
                    control.setProperty(propertyName, propertyValue);
                }
            }
        }

        /// <summary>
        /// 设置脚本
        /// </summary>
        /// <param name="text">脚本</param>
        public void setText(String text)
        {
            m_text = text;
            m_script = NFunctionEx.createIndicator(text, m_xml);
        }
    }
}
