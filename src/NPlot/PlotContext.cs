using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Florence
{
    public class PlotContext
    {
        protected enum PlotState { Ready, Hidden, Closed }

        public IPlotSurface2D PlotSurface { get; private set; }
        
        protected PlotState State { get; set; }

        public PlotContext(IPlotSurface2D plot_surface)
        {
            this.PlotSurface = plot_surface;
            this.State = PlotState.Ready;
        }

        public void scatter(IEnumerable<double> x, IEnumerable<double> y, string x_label="X", string y_label="Y", string title="")
        {
            this.invokeOnGuiThread(() => scatter_impl(x, y, x_label, y_label, title));
        }

        protected void scatter_impl(IEnumerable<double> x, IEnumerable<double> y, string x_label = "X", string y_label = "Y", string title = "")
        {
            PointPlot pp = new PointPlot();
            pp.OrdinateData = y;
            pp.AbscissaData = x;
            pp.Marker = new Marker(Marker.MarkerType.FilledCircle, 4, new Pen(Color.Blue));
            this.PlotSurface.Add(pp, Florence.PlotSurface2D.XAxisPosition.Bottom, Florence.PlotSurface2D.YAxisPosition.Left);
            
            this.refresh();
        }

        public void clear()
        {
            this.PlotSurface.Clear();
        }

        public virtual void hide()
        {

        }

        public virtual void close()
        {

        }

        public virtual void refresh()
        {

        }

        public virtual void invokeOnGuiThread(Action action)
        {
            action();
        }

    }
}
