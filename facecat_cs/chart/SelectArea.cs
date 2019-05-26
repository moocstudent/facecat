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

namespace FaceCat {
    /// <summary>
    /// 选中区域
    /// </summary>
    public class SelectArea : FCProperty {
        /// <summary>
        /// 析构函数
        /// </summary>
        ~SelectArea() {
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

        protected long m_backColor = FCColor.None;

        /// <summary>
        /// 获取或设置背景色
        /// </summary>
        public virtual long BackColor {
            get { return m_backColor; }
            set { m_backColor = value; }
        }

        protected FCRect m_bounds;

        /// <summary>
        /// 获取或设置选中框的区域
        /// </summary>
        public virtual FCRect Bounds {
            get { return m_bounds; }
            set { m_bounds = value; }
        }

        protected bool m_canResize;

        /// <summary>
        /// 获取或设置是否可以改变选中框的大小
        /// </summary>
        public virtual bool CanResize {
            get { return m_canResize; }
            set { m_canResize = value; }
        }

        protected bool m_enabled = true;

        /// <summary>
        /// 获取或设置是否可以出现选中框
        /// </summary>
        public virtual bool Enabled {
            get { return m_enabled; }
            set { m_enabled = value; }
        }

        protected bool m_isDeleted;

        /// <summary>
        /// 获取或设置是否已被销毁
        /// </summary>
        public virtual bool IsDeleted {
            get { return m_isDeleted; }
        }

        protected long m_lineColor = FCColor.argb(255, 255, 255);

        /// <summary>
        /// 获取或设置选中框的颜色
        /// </summary>
        public virtual long LineColor {
            get { return m_lineColor; }
            set { m_lineColor = value; }
        }

        protected bool m_visible;

        /// <summary>
        /// 获取或设置是否显示选中框
        /// </summary>
        public virtual bool Visible {
            get { return m_visible; }
            set { m_visible = value; }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void close() {
            m_visible = false;
            m_canResize = false;
        }

        /// <summary>
        /// 销毁资源
        /// </summary>
        public void delete() {
            m_isDeleted = true;
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
            else if (name == "enabled") {
                type = "bool";
                value = FCStr.convertBoolToStr(Enabled);
            }
            else if (name == "linecolor") {
                type = "color";
                value = FCStr.convertColorToStr(LineColor);
            }
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns></returns>
        public virtual ArrayList<String> getPropertyNames() {
            ArrayList<String> propertyNames = new ArrayList<String>();
            propertyNames.AddRange(new String[] { "AllowUserPaint", "Enabled", "LineColor" });
            return propertyNames;
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
            else if (name == "enabled") {
                Enabled = FCStr.convertStrToBool(value);
            }
            else if (name == "linecolor") {
                LineColor = FCStr.convertStrToColor(value);
            }
        }
    }
}
