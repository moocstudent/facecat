/*捂脸猫FaceCat框架 v1.0
 1.创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 3.联合创始人-河北思尔企业管理咨询有限公司合伙人-肖添龙(微信号:xiaotianlong_luu);
 4.联合开发者-陈晓阳(微信号:chenxiaoyangzxy)，助理-朱炜(微信号:cnnic_zhu);
 5.该框架开源协议为BSD，欢迎对我们的创业活动进行各种支持，欢迎更多开发者加入。
 包含C/C++,Java,C#,iOS,MacOS,Linux六个版本的图形和通讯服务框架。
 */
package facecat.topin.plot;

import facecat.topin.chart.*;

/**
 * 画线工具工厂类
 */
public class PFactory {

    /**
     * 根据类型创建线条
     *
     * @param plotType 类型
     * @returns 画线工具对象
     */
    public static FCPlot createPlot(String plotType) {
        FCPlot iplot = null;
        if (plotType.equals("ANDREWSPITCHFORK")) {
            iplot = new AndrewsPitchfork();
        } else if (plotType.equals("ANGLELINE")) {
            iplot = new Angleline();
        } else if (plotType.equals("CIRCUMCIRCLE")) {
            iplot = new CircumCircle();
        } else if (plotType.equals("ARROWSEGMENT")) {
            iplot = new ArrowSegment();
        } else if (plotType.equals("DOWNARROW")) {
            iplot = new DownArrow();
        } else if (plotType.equals("DROPLINE")) {
            iplot = new Dropline();
        } else if (plotType.equals("ELLIPSE")) {
            iplot = new Ellipse();
        } else if (plotType.equals("FIBOELLIPSE")) {
            iplot = new FiboEllipse();
        } else if (plotType.equals("FIBOFANLINE")) {
            iplot = new FiboFanline();
        } else if (plotType.equals("FIBOTIMEZONE")) {
            iplot = new FiboTimezone();
        } else if (plotType.equals("GANNBOX")) {
            iplot = new GannBox();
        } else if (plotType.equals("GANNLINE")) {
            iplot = new GannLine();
        } else if (plotType.equals("GOLDENRATIO")) {
            iplot = new GoldenRatio();
        } else if (plotType.equals("HLINE")) {
            iplot = new HLine();
        } else if (plotType.equals("LEVELGRADING")) {
            iplot = new LevelGrading();
        } else if (plotType.equals("LINE")) {
            iplot = new Line();
        } else if (plotType.equals("LRBAND")) {
            iplot = new Lrband();
        } else if (plotType.equals("LRCHANNEL")) {
            iplot = new LrChannel();
        } else if (plotType.equals("LRLINE")) {
            iplot = new Lrline();
        } else if (plotType.equals("NullPoint")) {
            iplot = new NullPoint();
        } else if (plotType.equals("PARALLEL")) {
            iplot = new Parallel();
        } else if (plotType.equals("PERCENT")) {
            iplot = new Percent();
        } else if (plotType.equals("PERIODIC")) {
            iplot = new Periodic();
        } else if (plotType.equals("PRICE")) {
            iplot = new Price();
        } else if (plotType.equals("RANGERULER")) {
            iplot = new RangeRuler();
        } else if (plotType.equals("RASELINE")) {
            iplot = new RaseLine();
        } else if (plotType.equals("RAY")) {
            iplot = new Ray();
        } else if (plotType.equals("FCRect")) {
            iplot = new RectLine();
        } else if (plotType.equals("SEGMENT")) {
            iplot = new Segment();
        } else if (plotType.equals("SINE")) {
            iplot = new Sine();
        } else if (plotType.equals("SPEEDRESIST")) {
            iplot = new SpeedResist();
        } else if (plotType.equals("SECHANNEL")) {
            iplot = new SeChannel();
        } else if (plotType.equals("SYMMETRICLINE")) {
            iplot = new SymmetricLine();
        } else if (plotType.equals("SYMMETRICTRIANGLE")) {
            iplot = new SymemetrictriAngle();
        } else if (plotType.equals("TIMERULER")) {
            iplot = new TimeRuler();
        } else if (plotType.equals("TRIANGLE")) {
            iplot = new Triangle();
        } else if (plotType.equals("UPARROW")) {
            iplot = new UpArrow();
        } else if (plotType.equals("VLINE")) {
            iplot = new VLine();
        } else if (plotType.equals("WAVERULER")) {
            iplot = new WaveRuler();
        } else if (plotType.equals("TIRONELEVELS")) {
            iplot = new TironeLevels();
        } else if (plotType.equals("RAFFCHANNEL")) {
            iplot = new RaffChannel();
        } else if (plotType.equals("QUADRANTLINES")) {
            iplot = new QuadrantLines();
        } else if (plotType.equals("BOXLINE")) {
            iplot = new BoxLine();
        } else if (plotType.equals("PARALLELOGRAM")) {
            iplot = new ParalleGram();
        } else if (plotType.equals("CIRCLE")) {
            iplot = new Circle();
        } else if (plotType.equals("PRICECHANNEL")) {
            iplot = new PriceChannel();
        } else if (plotType.equals("GP")) {
            iplot = new Gp();
        } else if (plotType.equals("GA")) {
            iplot = new Ga();
        }
        return iplot;
    }
}
