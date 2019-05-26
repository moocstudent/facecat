using System.Drawing;
using System.Windows.Forms;
using ScintillaNet;
namespace FaceCat
{
    partial class ScintillaX
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.Encoding = System.Text.Encoding.Default;
            this.Indentation.TabWidth = 4;
            this.Margins.Margin0.Width = 20;
            this.Margins.Margin0.Type = MarginType.Symbol;
            this.Margins.Margin1.Width = 25;
            this.Margins.Margin1.Type = MarginType.Number;
            this.Margins.Margin2.Width = 20;
            this.Margins.FoldMarginColor = this.BackColor;
            this.Styles.BraceBad.FontName = "Verdana";
            this.Styles.BraceLight.FontName = "Verdana";
            this.Styles.ControlChar.FontName = "Verdana";
            this.Styles.Default.FontName = "Verdana";
            this.Styles.IndentGuide.FontName = "Verdana";
            this.Styles.LastPredefined.FontName = "Verdana";
            this.Styles.LineNumber.FontName = "Verdana";
            this.Styles.Max.FontName = "Verdana";
            this.IsBraceMatching = true;
            this.ConfigurationManager.Language = "html";
        }
        #endregion
    }
}
