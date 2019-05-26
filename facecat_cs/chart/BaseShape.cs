/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-捂脸鹿创始人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Collections.Generic;

namespace FaceCat {
    /// <summary>
    /// 所有线条的父类
    /// </summary>
    [Serializable()]
    public class BaseShape : FCProperty {
        /// <summary>
        /// 析构函数
        /// </summary>
        ~BaseShape() {
            delete();
        }

        protected bool m_allowUserPaint;

        /// <summary>
        /// 获取或设置是否允许用户绘图
        /// </summary>
        public virtual bool AllowUserPaint {
            get { return m_allowUserPaint; }
            set { m_allowUserPaint = value; }
        }

        protected AttachVScale m_attachVScale = AttachVScale.Left;

        /// <summary>
        /// 获取或设置依附于左轴还是右轴
        /// </summary>
        public virtual AttachVScale AttachVScale {
            get { return m_attachVScale; }
            set { m_attachVScale = value; }
        }

        protected bool m_isDeleted;

        /// <summary>
        /// 获取或设置是否已被销毁
        /// </summary>
        public virtual bool IsDeleted {
            get { return m_isDeleted; }
        }

        protected bool m_selected = false;

        /// <summary>
        /// 获取或设置是否被选中
        /// </summary>
        public virtual bool Selected {
            get { return m_selected; }
            set { m_selected = value; }
        }

        protected bool m_visible = true;

        /// <summary>
        /// 获取或设置图形是否可见
        /// </summary>
        public virtual bool Visible {
            get { return m_visible; }
            set { m_visible = value; }
        }

        protected int m_zOrder;

        /// <summary>
        /// 获取或设置绘图顺序
        /// </summary>
        public virtual int ZOrder {
            get { return m_zOrder; }
            set { m_zOrder = value; }
        }

        /// <summary>
        /// 销毁资源的方法
        /// </summary>
        public virtual void delete() {
            if (!m_isDeleted) {
                m_isDeleted = true;
            }
        }

        /// <summary>
        /// 获取基础字段
        /// </summary>
        /// <returns></returns>
        public virtual int getBaseField() {
            return -1;
        }

        /// <summary>
        /// 由字段名称获取字段标题
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <returns>字段标题</returns>
        public virtual String getFieldText(int fieldName) {
            return "";
        }

        /// <summary>
        /// 获取所有字段
        /// </summary>
        /// <returns></returns>
        public virtual int[] getFields() {
            return null;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public virtual void getProperty(String name, ref String value, ref String type) {
            if (name == "allowuserpaint") {
                type = "bool";
                value = FCStr.convertBoolToStr(AllowUserPaint);
            }
            else if (name == "attachvscale") {
                type = "enum:AttachVScale";
                if (AttachVScale == AttachVScale.Left) {
                    value = "Left";
                }
                else {
                    value = "Right";
                }
            }
            else if (name == "selected") {
                type = "bool";
                value = FCStr.convertBoolToStr(Selected);
            }
            else if (name == "visible") {
                type = "bool";
                value = FCStr.convertBoolToStr(Visible);
            }
            else if (name == "zorder") {
                type = "int";
                value = FCStr.convertIntToStr(ZOrder);
            }
            else {
                type = "undefined";
                value = "";
            }
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns></returns>
        public virtual ArrayList<String> getPropertyNames() {
            ArrayList<String> propertyNames = new ArrayList<String>();
            propertyNames.AddRange(new String[] { "AllowUserPaint", "AttachVScale", "Selected", "Visible", "ZOrder" });
            return propertyNames;
        }

        /// <summary>
        /// 获取选中点的颜色
        /// </summary>
        /// <returns>颜色</returns>
        public virtual long getSelectedColor() {
            return 0;
        }

        /// <summary>
        /// 重绘方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="div">图层</param>
        /// <param name="rect">区域</param>
        public virtual void onPaint(FCPaint paint, ChartDiv div, FCRect rect) {

        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public virtual void setProperty(String name, String value) {
            if (name == "allowuserpaint") {
                AllowUserPaint = FCStr.convertStrToBool(value);
            }
            else if (name == "attachvscale") {
                value = value.ToLower();
                if (value == "left") {
                    AttachVScale = AttachVScale.Left;
                }
                else {
                    AttachVScale = AttachVScale.Right;
                }
            }
            else if (name == "selected") {
                Selected = FCStr.convertStrToBool(value);
            }
            else if (name == "visible") {
                Visible = FCStr.convertStrToBool(value);
            }
            else if (name == "zorder") {
                ZOrder = FCStr.convertStrToInt(value);
            }
        }
    }
}
