using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Florence.WinForms
{
    public partial class PlotControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.coordinates_ = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // WinFormsPlotSurface2D
            // 
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.DoubleBuffered = true;
            this.Name = "WinFormsPlotSurface2D";
            this.Size = new System.Drawing.Size(328, 272);
            this.ResumeLayout(false);

        }


        private System.Windows.Forms.ToolTip coordinates_;
        
    }
}
