using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Florence;

namespace SimpleTestCommon
{
    public static class SimpleTestCommon
    {
        public static void Run(ImperativeHost plot)
        {
            plot.Start();

            var x = new double[] { 1.0, 2.0, 3.0 };
            var y = new double[] { 1.0, 2.0, 3.0 };
            var z = new double[] { 0.0, 2.0, 4.0 };
            var plot1 = new PointPlot();
            plot1.AbscissaData = x;
            plot1.OrdinateData = y;
            plot.Add(plot1);
            plot.ActiveFigure.refresh();

            Console.ReadLine();

            var plot2 = new LinePlot(y, x);
            plot.Title = "Test Plot 2";
            plot.XAxis1.Label = "X2";
            plot.YAxis1.Label = "Y2";

            plot.Add(plot2);

            Console.ReadLine();

            plot.Stop();
        }
    }
}
