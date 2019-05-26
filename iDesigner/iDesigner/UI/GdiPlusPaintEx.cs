/*基于捂脸猫FaceCat框架 v1.0
 捂脸猫创始人-矿洞程序员-脉脉KOL-陶德 (微信号:suade1984);
 */

using System;
using System.Collections.Generic;
using System.Text;
using FaceCat;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace FaceCat
{
    /// <summary>
    /// 绘图方法
    /// </summary>
    public class GdiPlusPaintEx : GdiPlusPaint
    {
        /// <summary>
        /// 获取颜色
        /// </summary>
        /// <param name="dwPenColor">输入颜色</param>
        /// <returns>输出颜色</returns>
        public override long getPaintColor(long dwPenColor)
        {
            return FCDraw.GetColor(dwPenColor);
        }
    }
}
