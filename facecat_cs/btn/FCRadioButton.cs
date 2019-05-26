/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */

using System;
using System.Collections.Generic;

namespace FaceCat {
    /// <summary>
    /// 复选框控件
    /// </summary>
    public class FCRadioButton : FCCheckBox {
        /// <summary>
        /// 创建复选框
        /// </summary>
        public FCRadioButton() {
        }

        protected String groupName;

        /// <summary>
        /// 获取或设置组名
        /// </summary>
        public virtual String GroupName {
            get { return groupName; }
            set { groupName = value; }
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "RadioButton";
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">返回属性值</param>
        /// <param name="type">返回属性类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "groupname") {
                type = "text";
                value = GroupName;
            }
            else {
                base.getProperty(name, ref value, ref type);
            }
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns>属性名称列表</returns>
        public override ArrayList<String> getPropertyNames() {
            ArrayList<String> propertyNames = base.getPropertyNames();
            propertyNames.AddRange(new String[] { "GroupName" });
            return propertyNames;
        }

        /// <summary>
        /// 点击方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public override void onClick(FCTouchInfo touchInfo) {
            if (!Checked) {
                Checked = !Checked;
            }
            callTouchEvents(FCEventID.CLICK, touchInfo);
            invalidate();
        }

        /// <summary>
        /// 重绘选中按钮方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintCheckButton(FCPaint paint, FCRect clipRect) {
            //绘制背景图
            String bkImage = getPaintingBackImage();
            if (bkImage != null && bkImage.Length > 0) {
                paint.drawImage(bkImage, clipRect);
            }
            else {
                if (Checked) {
                    FCRect innerRect = new FCRect(clipRect.left + 2, clipRect.top + 2, clipRect.right - 3, clipRect.bottom - 3);
                    if (clipRect.right - clipRect.left < 4 || clipRect.bottom - clipRect.top < 4) {
                        innerRect = clipRect;
                    }
                    paint.fillEllipse(getPaintingButtonBackColor(), innerRect);
                }
                paint.drawEllipse(getPaintingButtonBorderColor(), 1, 0, clipRect);
            }
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public override void setProperty(String name, String value) {
            if (name == "groupname") {
                GroupName = value;
            }
            else {
                base.setProperty(name, value);
            }
        }

        /// <summary>
        /// 更新布局方法
        /// </summary>
        public override void update() {
            if (Checked) {
                ArrayList<FCView> controls = null;
                if (Parent != null) {
                    controls = Parent.getControls();
                }
                else {
                    controls = Native.getControls();
                }
                //反选组别相同的项
                int controlSize = controls.size();
                for (int i = 0; i < controlSize; i++) {
                    FCRadioButton radioButton = controls.get(i) as FCRadioButton;
                    if (radioButton != null && radioButton != this) {
                        if (radioButton.GroupName == GroupName && radioButton.Checked == true) {
                            radioButton.Checked = false;
                            radioButton.invalidate();
                        }
                    }
                }
            }
        }
    }
}
