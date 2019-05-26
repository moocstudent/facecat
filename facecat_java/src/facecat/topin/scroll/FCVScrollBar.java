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
 * 纵向滚动条控件
 */
public class FCVScrollBar extends FCScrollBar {

    /**
     * 创建控件
     */
    public FCVScrollBar() {
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
        return "VScrollBar";
    }

    /**
     * 拖动滚动方法
     */
    @Override
    public void onDragScroll() {
        boolean floatRight = false;
        FCButton backButton = getBackButton();
        FCButton scrollButton = getScrollButton();
        int backButtonHeight = backButton.getHeight();
        int contentSize = getContentSize();
        if (scrollButton.getBottom() > backButtonHeight) {
            floatRight = true;
        }
        super.onDragScroll();
        if (floatRight) {
            setPos(contentSize);
        } else {
            setPos((int) ((long) contentSize * (long) scrollButton.getTop() / backButton.getHeight()));
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
        if (mp.y < scrollButton.getTop()) {
            pagereduce();
            setisReducing(true);
        } else if (mp.y > scrollButton.getBottom()) {
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

        if (contentSize > 0 && addButton != null && backButton != null && reduceButton != null && scrollButton != null) {
            int pos = getPos();
            int pageSize = getPageSize();
            if (pos > contentSize - pageSize) {
                pos = contentSize - pageSize;
            }
            if (pos < 0) {
                pos = 0;
            }
            int abHeight = addButton.isVisible() ? addButton.getHeight() : 0;
            addButton.setSize(new FCSize(width, abHeight));
            addButton.setLocation(new FCPoint(0, height - abHeight));
            int rbHeight = reduceButton.isVisible() ? reduceButton.getHeight() : 0;
            reduceButton.setSize(new FCSize(width, rbHeight));
            reduceButton.setLocation(new FCPoint(0, 0));
            int backHeight = height - abHeight - rbHeight;
            backButton.setSize(new FCSize(width, backHeight));
            backButton.setLocation(new FCPoint(0, rbHeight));
            int scrollHeight = backHeight * pageSize / contentSize;
            int scrollPos = (int) ((long) backHeight * (long) pos / contentSize);
            if (scrollHeight < 10) {
                scrollHeight = 10;
                if (scrollPos + scrollHeight > backHeight) {
                    scrollPos = backHeight - scrollHeight;
                }
            }
            scrollButton.setSize(new FCSize(width, scrollHeight));
            scrollButton.setLocation(new FCPoint(0, scrollPos));
        }
    }
}
