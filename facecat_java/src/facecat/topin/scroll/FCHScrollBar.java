/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.scroll;

import facecat.topin.btn.*;
import facecat.topin.core.*;

/**
 * 横向滚动条控件
 */
public class FCHScrollBar extends FCScrollBar {

    /**
     * 创建控件
     */
    public FCHScrollBar() {
    }

    @Override
    public void callControlTouchEvent(int eventID, Object sender, FCTouchInfo touchInfo) {
        super.callControlTouchEvent(eventID, sender, touchInfo.clone());
        if (sender == getBackButton()) {
            if (eventID == FCEventID.TOUCHDOWN) {
                onBackButtonTouchDown(touchInfo.clone());
            } else if (eventID == FCEventID.TOUCHUP) {
                onBackButtonTouchUp(touchInfo.clone());
            }
        }
    }

    /**
     * 销毁方法
     */
    @Override
    public void delete() {
        if (!isDeleted()) {
            FCButton backButton = getBackButton();
            if (backButton != null) {
                backButton.removeEvent(this, FCEventID.TOUCHDOWN);
                backButton.removeEvent(this, FCEventID.TOUCHUP);
            }
        }
        super.delete();
    }

    /**
     * 获取控件类型
     */
    @Override
    public String getControlType() {
        return "HScrollBar";
    }

    @Override
    public void onDragScroll() {
        boolean floatRight = false;
        FCButton backButton = getBackButton();
        FCButton scrollButton = getScrollButton();
        int backButtonWidth = backButton.getWidth();
        int contentSize = getContentSize();
        if (scrollButton.getRight() > backButtonWidth) {
            floatRight = true;
        }
        super.onDragScroll();
        if (floatRight) {
            setPos(contentSize);
        } else {
            setPos((int) ((long) contentSize * (long) scrollButton.getLeft() / backButtonWidth));
        }
        onScrolled();
    }

    /**
     * 添加控件方法
     */
    @Override
    public void onLoad() {
        boolean isAdd = false;
        FCButton backButton = getBackButton();
        if (backButton != null) {
            isAdd = true;
        }
        super.onLoad();
        if (!isAdd) {
            backButton = getBackButton();
            backButton.addEvent(this, FCEventID.TOUCHDOWN);
            backButton.addEvent(this, FCEventID.TOUCHUP);
        }
    }

    /**
     * 滚动条背景按钮触摸按下回调方法
     *
     * @param touchInfo 触摸信息
     */
    public void onBackButtonTouchDown(FCTouchInfo touchInfo) {
        FCButton scrollButton = getScrollButton();
        FCPoint mp = touchInfo.m_firstPoint.clone();
        if (mp.x < scrollButton.getLeft()) {
            pagereduce();
            setisReducing(true);
        } else if (mp.x > scrollButton.getRight()) {
            pageadd();
            setisAdding(true);
        }
    }

    /**
     * 滚动条背景按钮触摸抬起方法
     *
     * @param touchInfo 触摸信息
     */
    public void onBackButtonTouchUp(FCTouchInfo touchInfo) {
        setisAdding(false);
        setisReducing(false);
    }

    /**
     * 重新布局方法
     */
    @Override
    public void update() {
        if (getNative() == null) {
            return;
        }
        FCButton addButton = getAddButton();
        FCButton backButton = getBackButton();
        FCButton reduceButton = getReduceButton();
        FCButton scrollButton = getScrollButton();
        int width = getWidth(), height = getHeight();
        int contentSize = getContentSize();
        // 设置按钮的位置
        if (contentSize > 0 && addButton != null && backButton != null && reduceButton != null && scrollButton != null) {
            int pos = getPos();
            int pageSize = getPageSize();
            if (pos > contentSize - pageSize) {
                pos = contentSize - pageSize;
            }
            if (pos < 0) {
                pos = 0;
            }
            int abWidth = addButton.isVisible() ? addButton.getWidth() : 0;
            addButton.setSize(new FCSize(abWidth, height));
            addButton.setLocation(new FCPoint(width - abWidth, 0));
            int rbWidth = reduceButton.isVisible() ? reduceButton.getWidth() : 0;
            reduceButton.setSize(new FCSize(rbWidth, height));
            reduceButton.setLocation(new FCPoint(0, 0));
            int backWidth = width - abWidth - rbWidth;
            backButton.setSize(new FCSize(backWidth, height));
            backButton.setLocation(new FCPoint(rbWidth, 0));
            int scrollWidth = backWidth * pageSize / contentSize;
            int scrollPos = backWidth * pos / contentSize;
            if (scrollWidth < 10) {
                scrollWidth = 10;
                if (scrollPos + scrollWidth > backWidth) {
                    scrollPos = backWidth - scrollWidth;
                }
            }
            scrollButton.setSize(new FCSize(scrollWidth, height));
            scrollButton.setLocation(new FCPoint(scrollPos, 0));
        }
    }
}
