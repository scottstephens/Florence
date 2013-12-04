using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Florence;
using System.Threading;

namespace SimpleTestCommon
{
    public static class SimpleTestCommon
    {
        public static void Run(InteractiveHost plot)
        {
            // All the toolkit specific code does is one of these lines:
            // InteractiveHost plot = new Florence.GtkSharp.InteractiveHost();
            // InteractiveHost plot = new Florence.WinForms.InteractiveHost();

            // Start the interactive host; this starts up a GUI thread
            // and otherwise gets the GUI ready to go.
            plot.Start();


            // Generate some data to plot!
            var rand = new Random(0);
            
            var x = new double[100];
            for (int ii = 0; ii < x.Length; ++ii)
                x[ii] = rand.NextNormal(0.0, 1.0);

            var y = new double[x.Length];
            for (int ii = 0; ii < y.Length; ++ii)
                y[ii] = 2.0 + 3.0 * x[ii] + rand.NextNormal(0.0, 2.5);

            var t = new DateTime[15];
            var s = new double[t.Length];
            t[0] = new DateTime(2013,12,1);
            s[0] = 100.0;

            for (int ii = 1; ii < t.Length; ++ii)
            {
                t[ii] = t[ii-1].AddDays(1);
                s[ii] = s[ii - 1]*Math.Exp(rand.NextNormal(0.05 / 365.0, 0.15 / Math.Sqrt(365.0)));
            }


            // Create our first plot
            var plot1 = new PointPlot() { AbscissaData = x, OrdinateData = y };
            plot.Add(plot1);
            plot.XAxis1.Label = "Love of Graphs";
            plot.YAxis1.Label = "Overall Awesomeness";
            plot.Title = "Effect of Graph Affinity on Overall Awesomeness";

            // Pause execution. In an interactive environment, (C#/F# REPL) this would not be necessary
            Console.ReadLine();

            // Try putting your mouse over the numbers on either the X or Y axis, 
            // pressing the mouse button then dragging left and right (for the X)
            // or up and down (for the Y).

            // Now try clicking and dragging in the main plot area.

            // Now press enter, to move on to the next example

            plot.newFigure();
            plot.Title = "A new one!";

            Console.ReadLine();

            plot.previous();
            plot.Title = "First one again!";

            Console.ReadLine();

            plot.next();
            plot.closeFigure();

            var line_plot = plot.newFigure();

            line_plot.Add(new LinePlot() { AbscissaData = t, OrdinateData = s });

            //var plot2 = new LinePlot() { AbscissaData = x, OrdinateData = y };
            //plot.Title = "Test Plot 2";
            //plot.XAxis1.Label = "X2";
            //plot.YAxis1.Label = "Y2";
            //plot.Add(plot2);

            Console.ReadLine();

            plot.Stop();
        }

        // Generate pseudo-random approximately normal random variable
        // using Box-Muller transformation
        public static double NextNormal(this Random rand, double mean, double sd)
        {
            double u1 = rand.NextDouble();
            double u2 = rand.NextDouble();

            double z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

            return mean + sd * z;
        }
    }
}
