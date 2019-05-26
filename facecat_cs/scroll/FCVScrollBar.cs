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
    /// 纵向滚动条控件
    /// </summary>
    public class FCVScrollBar : FCScrollBar {
        /// <summary>
        /// 创建控件
        /// </summary>
        public FCVScrollBar() {
            m_backButtonTouchDownEvent = new FCTouchEvent(backButtonTouchDown);
            m_backButtonTouchUpEvent = new FCTouchEvent(backButtonTouchUp);
        }

        /// <summary>
        /// 背景按钮的触摸按下事件
        /// </summary>
        private FCTouchEvent m_backButtonTouchDownEvent;

        /// <summary>
        /// 背景按钮的触摸抬起事件
        /// </summary>
        private FCTouchEvent m_backButtonTouchUpEvent;

        /// <summary>
        /// 滚动条背景按钮触摸按下回调事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="touchInfo">触摸信息</param>
        private void backButtonTouchDown(object sender, FCTouchInfo touchInfo) {
            onBackButtonTouchDown(touchInfo);
        }

        /// <summary>
        /// 滚动条背景按钮触摸抬起回调事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="touchInfo">触摸信息</param>
        private void backButtonTouchUp(object sender, FCTouchInfo touchInfo) {
            onBackButtonTouchUp(touchInfo);
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public override void delete() {
            if (!IsDeleted) {
                FCButton backButton = BackButton;
                if (backButton != null) {
                    if (m_backButtonTouchDownEvent != null) {
                        backButton.removeEvent(m_backButtonTouchDownEvent, FCEventID.TOUCHDOWN);
                        m_backButtonTouchDownEvent = null;
                    }
                    if (m_backButtonTouchUpEvent != null) {
                        backButton.removeEvent(m_backButtonTouchUpEvent, FCEventID.TOUCHUP);
                        m_backButtonTouchUpEvent = null;
                    }
                }
            }
            base.delete();
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <returns>控件类型</returns>
        public override String getControlType() {
            return "VScrollBar";
        }

        /// <summary>
        /// 拖动滚动方法
        /// </summary>
        public override void onDragScroll() {
            bool floatRight = false;
            FCButton backButton = BackButton;
            FCButton scrollButton = ScrollButton;
            int backButtonHeight = backButton.Height;
            int contentSize = ContentSize;
            if (scrollButton.Bottom > backButtonHeight) {
                floatRight = true;
            }
            base.onDragScroll();
            if (floatRight) {
                Pos = contentSize;
            }
            else {
                Pos = (int)(((long)contentSize * (long)scrollButton.Top) / backButton.Height);
            }
            onScrolled();
        }

        /// <summary>
        /// 添加控件方法
        /// </summary>
        public override void onLoad() {
            bool isAdd = false;
            FCButton backButton = BackButton;
            if (backButton != null) {
                isAdd = true;
            }
            base.onLoad();
            if (!isAdd) {
                backButton = BackButton;
                backButton.addEvent(m_backButtonTouchDownEvent, FCEventID.TOUCHDOWN);
                backButton.addEvent(m_backButtonTouchUpEvent, FCEventID.TOUCHUP);
            }
        }

        /// <summary>
        /// 滚动条背景按钮触摸按下回调方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public void onBackButtonTouchDown(FCTouchInfo touchInfo) {
            FCButton scrollButton = ScrollButton;
            FCPoint mp = touchInfo.m_firstPoint;
            if (mp.y < scrollButton.Top) {
                pageReduce();
                IsReducing = true;
            }
            else if (mp.y > scrollButton.Bottom) {
                pageAdd();
                IsAdding = true;
            }
        }

        /// <summary>
        /// 滚动条背景按钮触摸抬起方法
        /// </summary>
        /// <param name="touchInfo">触摸信息</param>
        public void onBackButtonTouchUp(FCTouchInfo touchInfo) {
            IsAdding = false;
            IsReducing = false;
        }

        /// <summary>
        /// 重新布局方法
        /// </summary>
        public override void update() {
            if (Native == null) {
                return;
            }
            FCButton addButton = AddButton;
            FCButton backButton = BackButton;
            FCButton reduceButton = ReduceButton;
            FCButton scrollButton = ScrollButton;
            int width = Width, height = Height;
            int contentSize = ContentSize;
            //设置按钮的位置
            if (contentSize > 0 && addButton != null && backButton != null && reduceButton != null && scrollButton != null) {
                int pos = Pos;
                int pageSize = PageSize;
                if (pos > contentSize - pageSize) {
                    pos = contentSize - pageSize;
                }
                if (pos < 0) {
                    pos = 0;
                }
                int abHeight = addButton.Visible ? addButton.Height : 0;
                addButton.Size = new FCSize(width, abHeight);
                addButton.Location = new FCPoint(0, height - abHeight);
                int rbHeight = reduceButton.Visible ? reduceButton.Height : 0;
                reduceButton.Size = new FCSize(width, rbHeight);
                reduceButton.Location = new FCPoint(0, 0);
                int backHeight = height - abHeight - rbHeight;
                backButton.Size = new FCSize(width, backHeight);
                backButton.Location = new FCPoint(0, rbHeight);
                //获取滚动条宽度和坐标
                int scrollHeight = backHeight * pageSize / contentSize;
                int scrollPos = (int)((long)backHeight * (long)pos / contentSize);
                if (scrollHeight < 10) {
                    scrollHeight = 10;
                    if (scrollPos + scrollHeight > backHeight) {
                        scrollPos = backHeight - scrollHeight;
                    }
                }
                scrollButton.Size = new FCSize(width, scrollHeight);
                scrollButton.Location = new FCPoint(0, scrollPos);
            }
        }
    }
}
