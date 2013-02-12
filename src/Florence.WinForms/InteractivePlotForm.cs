using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Florence.WinForms
{
    public partial class InteractivePlotForm : Form
    {
        public  Florence.WinForms.WinFormsPlotSurface2D PlotSurface { get { return plotSurface; } }

        public InteractivePlotForm()
        {
            InitializeComponent();
        }
    }
}
