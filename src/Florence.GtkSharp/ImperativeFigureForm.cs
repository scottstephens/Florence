using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Florence.GtkSharp
{
    public class ImperativeFigureForm : Gtk.Window
    {
        public InteractivePlotSurface2D PlotSurface { get; set; }

        public ImperativeFigureForm(string title) : base(title)
        {
            this.InitializeComponent();
            this.PlotSurface = new InteractivePlotSurface2D();
            this.PlotWidget.InteractivePlotSurface2D = this.PlotSurface;
        }

        private PlotWidget PlotWidget;
        
        protected void InitializeComponent()
        {
            this.PlotWidget = new PlotWidget();

            this.SetSizeRequest(600, 500);

            this.Add(this.PlotWidget);
        }
    }
}
