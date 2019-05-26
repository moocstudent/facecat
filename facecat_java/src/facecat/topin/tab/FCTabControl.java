/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.tab;

import facecat.topin.div.*;
import facecat.topin.btn.*;
import facecat.topin.core.*;
import java.util.*;

/**
 * 多页夹控件
 */
public class FCTabControl extends FCDiv {

    /**
     * 创建多页夹
     */
    public FCTabControl() {
    }

    /**
     * 动画状态
     */
    protected int m_animationState;

    /**
     * 所有页
     */
    public ArrayList<FCTabPage> m_tabPages = new ArrayList<FCTabPage>();

    /**
     * 秒表ID
     */
    private int m_timerID = getNewTimerID();

    protected FCTabPageLayout m_layout = FCTabPageLayout.Top;

    /**
     * 获取页的布局位置
     */
    public FCTabPageLayout getLayout() {
        return m_layout;
    }

    /**
     * 设置页的布局位置
     */
    public void setLayout(FCTabPageLayout value) {
        m_layout = value;
    }

    protected int m_selectedIndex = -1;

    /**
     * 获取选中的索引
     */
    public int getSelectedIndex() {
        return m_selectedIndex;
    }

    /**
     * 设置选中的索引
     */
    public void setSelectedIndex(int value) {
        int tabPageSize = m_tabPages.size();
        if (tabPageSize > 0) {
            if (value >= 0 && value < tabPageSize) {
                m_selectedIndex = value;
                setSelectedTabPage(m_tabPages.get(value));
            }
        }
    }

    /**
     * 获取选中的页
     */
    public FCTabPage getSelectedTabPage() {
        int tabPageSize = m_tabPages.size();
        if (tabPageSize > 0) {
            if (m_selectedIndex >= 0 && m_selectedIndex < tabPageSize) {
                return m_tabPages.get(m_selectedIndex);
            }
        }
        return null;
    }

    /**
     * 设置选中的页
     */
    public void setSelectedTabPage(FCTabPage value) {
        int index = -1;
        int tabPageSize = m_tabPages.size();
        if (value != null && tabPageSize > 0) {
            FCTabPage oldSelectedTabPage = getSelectedTabPage();
            for (int i = 0; i < tabPageSize; i++) {
                FCTabPage tabPage = m_tabPages.get(i);
                if (tabPage == value) {
                    index = i;
                    tabPage.setVisible(true);
                } else {
                    tabPage.setVisible(false);
                }
            }
            if (index != -1) {
                m_selectedIndex = index;
            } else {
                if (tabPageSize > 0) {
                    m_selectedIndex = 0;
                }
            }
            FCTabPage newSelectedTabPage = getSelectedTabPage();
            if (oldSelectedTabPage != newSelectedTabPage) {
                onSelectedTabPageChanged();
            }
            newSelectedTabPage.bringToFront();
            newSelectedTabPage.getHeaderButton().bringToFront();
        } else {
            m_selectedIndex = -1;
        }
    }

    protected boolean m_useAnimation;

    /**
     * 获取是否使用动画
     */
    public boolean useAnimation() {
        return m_useAnimation;
    }

    /**
     * 设置是否使用动画
     */
    public void setUseAnimation(boolean value) {
        m_useAnimation = value;
        if (m_useAnimation) {
            startTimer(m_timerID, 20);
        } else {
            stopTimer(m_timerID);
        }
    }

    /**
     * 添加控件方法
     *
     * @param control 控件
     */
    @Override
    public void addControl(FCView control) {
        FCTabPage tabPage = (FCTabPage) ((control instanceof FCTabPage) ? control : null);
        if (tabPage != null) {
            tabPage.setTabControl(this);
        }
        super.addControl(control);
        if (tabPage != null) {
            m_tabPages.add(tabPage);
            setSelectedTabPage(tabPage);
        }
    }

    /**
     * 清除控件
     */
    @Override
    public void clearControls() {
        m_tabPages.clear();
        m_selectedIndex = -1;
        super.clearControls();
    }

    /**
     * 销毁方法
     */
    @Override
    public void delete() {
        if (!isDeleted()) {
            stopTimer(m_timerID);
        }
        super.delete();
    }

    /**
     * 绘制移动
     */
    protected void drawMoving() {
        if (m_animationState > 0) {
            boolean drawing = false;
            int tabPageSize = m_tabPages.size();
            FCTabPage selectedTabPage = getSelectedTabPage();
            for (int i = 0; i < tabPageSize; i++) {
                FCTabPage tabPage = m_tabPages.get(i);
                if (tabPage == selectedTabPage && m_animationState == 1) {
                    continue;
                }
                FCButton headerButton = tabPage.getHeaderButton();
                if (headerButton.isVisible()) {
                    int moving = headerButton.getLeft();
                    int pos = tabPage.getHeaderLocation().x;
                    if (m_layout == FCTabPageLayout.Left || m_layout == FCTabPageLayout.Right) {
                        pos = tabPage.getHeaderLocation().y;
                        moving = headerButton.getTop();
                    }
                    if (pos != moving) {
                        int relative = moving;
                        int sub = Math.abs(pos - relative);
                        int step = 20;
                        if (m_useAnimation) {
                            if (tabPage == selectedTabPage) {
                                if (sub > 200) {
                                    step = sub / 200 * 100;
                                }
                            } else {
                                step = sub;
                            }
                        } else {
                            step = sub;
                        }
                        if (relative != pos) {
                            if (pos > relative + step) {
                                relative += step;
                            } else if (pos < relative - step) {
                                relative -= step;
                            } else {
                                relative = pos;
                            }
                            if (m_layout == FCTabPageLayout.Left || m_layout == FCTabPageLayout.Right) {
                                headerButton.setTop(relative);
                            } else {
                                headerButton.setLeft(relative);
                            }
                            drawing = true;
                        }
                    }
                }
            }
            // 绘制
            if (!drawing) {
                if (m_animationState == 2) {
                    m_animationState = 0;
                }
            }
            update();
            invalidate();
        }
    }

    /**
     * 获取控件类型
     */
    @Override
    public String getControlType() {
        return "TabControl";
    }

    /**
     * 获取属性值
     *
     * @param name 属性名称
     * @param value 返回属性值
     * @param type 返回属性类型
     */
    @Override
    public void getProperty(String name, RefObject<String> value, RefObject<String> type) {
        if (name.equals("layout")) {
            type.argvalue = "enum:FCTabPageLayout";
            if (getLayout() == FCTabPageLayout.Left) {
                value.argvalue = "Left";
            } else if (getLayout() == FCTabPageLayout.Right) {
                value.argvalue = "Right";
            } else if (getLayout() == FCTabPageLayout.Bottom) {
                value.argvalue = "Bottom";
            } else {
                value.argvalue = "Top";
            }
        } else if (name.equals("selectedindex")) {
            type.argvalue = "int";
            value.argvalue = FCStr.convertIntToStr(getSelectedIndex());
        } else if (name.equals("useanimation")) {
            type.argvalue = "bool";
            value.argvalue = FCStr.convertBoolToStr(useAnimation());
        } else {
            super.getProperty(name, value, type);
        }
    }

    /**
     * 获取属性名称列表
     */
    @Override
    public ArrayList<String> getPropertyNames() {
        ArrayList<String> propertyNames = super.getPropertyNames();
        propertyNames.addAll(Arrays.asList(new String[]{"Layout", "SelectedIndex", "UseAnimation"}));
        return propertyNames;
    }

    /**
     * 获取多页夹列表
     */
    public ArrayList<FCTabPage> getTabPages() {
        return m_tabPages;
    }

    /**
     * 插入控件
     *
     * @param index 索引
     * @param control 控件
     */
    @Override
    public void insertControl(int index, FCView control) {
        FCTabPage tabPage = (FCTabPage) ((control instanceof FCTabPage) ? control : null);
        if (tabPage != null) {
            tabPage.setTabControl(this);
        }
        super.addControl(control);
        m_tabPages.add(index, tabPage);
        setSelectedTabPage(tabPage);
    }

    /**
     * 开始拖动页头
     */
    public void onDragTabHeaderBegin(FCTabPage tabPage) {
        m_animationState = 1;
        tabPage.getHeaderButton().bringToFront();
    }

    /**
     * 结束拖动页头
     *
     * @param tabPage 页
     */
    public void onDragTabHeaderEnd(FCTabPage tabPage) {
        if (m_animationState == 1) {
            m_animationState = 2;
            drawMoving();
        }
    }

    /**
     * 页头拖动中
     *
     * @param tabPage 页
     */
    public void onDraggingTabHeader(FCTabPage tabPage) {
        FCButton headerButton = tabPage.getHeaderButton();
        int moving = headerButton.getLeft();
        if (m_layout == FCTabPageLayout.Left || m_layout == FCTabPageLayout.Right) {
            moving = headerButton.getTop();
        }
        int tabPageSize = m_tabPages.size();
        for (int i = 0; i < tabPageSize; i++) {
            FCTabPage page = m_tabPages.get(i);
            if (page != tabPage) {
                FCButton tpHeader = page.getHeaderButton();
                if (tpHeader.isVisible()) {
                    int pos = page.getHeaderLocation().x;
                    int size = tpHeader.getWidth();
                    int sSize = headerButton.getWidth();
                    if (m_layout == FCTabPageLayout.Left || m_layout == FCTabPageLayout.Right) {
                        pos = page.getHeaderLocation().y;
                        size = tpHeader.getHeight();
                        sSize = headerButton.getHeight();
                    }
                    boolean instead = false;

                    if (moving > pos) {
                        if (moving > pos && moving < pos + size / 2) {
                            instead = true;
                        }
                    }
                    if (moving < pos) {
                        if (moving + sSize > pos + size / 2 && moving + sSize < pos + size) {
                            instead = true;
                        }
                    }

                    if (instead) {
                        FCPoint sLocation = tabPage.getHeaderLocation();
                        if (m_layout == FCTabPageLayout.Left || m_layout == FCTabPageLayout.Right) {
                            tabPage.setHeaderLocation(new FCPoint(tabPage.getHeaderLocation().x, pos));
                            page.setHeaderLocation(new FCPoint(page.getHeaderLocation().x, sLocation.y));
                        } else {
                            tabPage.setHeaderLocation(new FCPoint(pos, tabPage.getHeaderLocation().y));
                            page.setHeaderLocation(new FCPoint(sLocation.x, page.getHeaderLocation().y));
                        }
                        int oIndex = m_tabPages.indexOf(tabPage);
                        int nIndex = m_tabPages.indexOf(page);
                        m_tabPages.set(oIndex, page);
                        m_tabPages.set(nIndex, tabPage);
                        m_selectedIndex = nIndex;
                        break;
                    }
                }
            }
        }
        drawMoving();
    }

    /**
     * 添加控件方法
     */
    @Override
    public void onLoad() {
        super.onLoad();
        if (m_useAnimation) {
            startTimer(m_timerID, 20);
        } else {
            stopTimer(m_timerID);
        }
    }

    /**
     * 页改变方法
     */
    public void onSelectedTabPageChanged() {
        callEvents(FCEventID.SELECTEDTABPAGECHANGED);
    }

    @Override
    public void onTimer(int timerID) {
        super.onTimer(timerID);
        if (m_timerID == timerID) {
            drawMoving();
        }
    }

    /**
     * 移除控件
     *
     * @param control 控件
     */
    @Override
    public void removeControl(FCView control) {
        FCTabPage tabPage = (FCTabPage) ((control instanceof FCTabPage) ? control : null);
        if (tabPage != null) {
            int tabPageSize = m_tabPages.size();
            if (tabPageSize > 0) {
                FCTabPage selectedPage = getSelectedTabPage();
                if (selectedPage == tabPage) {
                    if (m_selectedIndex > 0) {
                        if (m_selectedIndex < tabPageSize - 1) {
                            selectedPage = m_tabPages.get(m_selectedIndex + 1);
                        } else {
                            selectedPage = m_tabPages.get(m_selectedIndex - 1);
                        }
                    } else {
                        if (tabPageSize > 1) {
                            selectedPage = m_tabPages.get(m_selectedIndex + 1);
                        }
                    }
                }
                m_tabPages.remove(tabPage);
                super.removeControl(tabPage.getHeaderButton());
                super.removeControl(tabPage);
                tabPage.setTabControl(null);
                setSelectedTabPage(selectedPage);
            }
        } else {
            super.removeControl(control);
        }
    }

    /**
     * 设置属性值
     *
     * @param name 属性名称
     * @param value 属性值
     */
    @Override
    public void setProperty(String name, String value) {
        if (name.equals("layout")) {
            value = value.toLowerCase();
            if (value.equals("left")) {
                setLayout(FCTabPageLayout.Left);
            } else if (value.equals("top")) {
                setLayout(FCTabPageLayout.Top);
            } else if (value.equals("right")) {
                setLayout(FCTabPageLayout.Right);
            } else if (value.equals("bottom")) {
                setLayout(FCTabPageLayout.Bottom);
            }
        } else if (name.equals("selectedindex")) {
            setSelectedIndex(FCStr.convertStrToInt(value));
        } else if (name.equals("useanimation")) {
            setUseAnimation(FCStr.convertStrToBool(value));
        } else {
            super.setProperty(name, value);
        }
    }

    /**
     * 重新布局
     */
    @Override
    public void update() {
        if (getNative() == null) {
            return;
        }
        super.update();
        FCPadding padding = getPadding();
        int left = padding.left, top = padding.top;
        int width = getWidth() - padding.left - padding.right;
        int height = getHeight() - padding.top - padding.bottom;
        int tabPageSize = m_tabPages.size();
        for (int i = 0; i < tabPageSize; i++) {
            FCTabPage tabPage = m_tabPages.get(i);
            FCButton headerButton = tabPage.getHeaderButton();
            if (headerButton.isVisible()) {
                FCPadding margin = headerButton.getMargin();
                int tw = headerButton.getWidth() + margin.left + margin.right;
                int th = headerButton.getHeight() + margin.top + margin.bottom;
                FCRect bounds = new FCRect();
                FCPoint headerLocation = new FCPoint();
                switch (m_layout) {
                    case Bottom:
                        bounds.left = padding.left;
                        bounds.top = padding.top;
                        bounds.right = width;
                        bounds.bottom = height - th;
                        headerLocation.x = left + margin.left;
                        headerLocation.y = height - th + margin.top;
                        break;
                    case Left:
                        bounds.left = tw;
                        bounds.top = padding.top;
                        bounds.right = width;
                        bounds.bottom = height;
                        headerLocation.x = padding.left + margin.left;
                        headerLocation.y = top + margin.top;
                        break;
                    case Right:
                        bounds.left = padding.left;
                        bounds.top = padding.top;
                        bounds.right = width - tw;
                        bounds.bottom = height;
                        headerLocation.x = width - tw + margin.left;
                        headerLocation.y = top + margin.top;
                        break;
                    case Top:
                        bounds.left = padding.left;
                        bounds.top = th;
                        bounds.right = width;
                        bounds.bottom = height;
                        headerLocation.x = left + margin.left;
                        headerLocation.y = padding.top + margin.top;
                        break;
                }
                tabPage.setBounds(bounds);
                tabPage.setHeaderLocation(headerLocation);
                if (!m_useAnimation) {
                    tabPage.getHeaderButton().setLocation(headerLocation);
                }

                if (m_animationState > 0) {
                    if (m_layout == FCTabPageLayout.Left || m_layout == FCTabPageLayout.Right) {
                        headerLocation.y = headerButton.getTop();
                    } else if (m_layout == FCTabPageLayout.Bottom || m_layout == FCTabPageLayout.Top) {
                        headerLocation.x = headerButton.getLeft();
                    }
                }
                headerButton.setLocation(headerLocation);
                left += tw;
                top += th;
            } else {
                tabPage.setVisible(false);
            }
        }
    }
}
