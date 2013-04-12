/*
 * Florence - A charting library for .NET
 * 
 * PlotSincFunction.cs
 * Copyright (C) 2003-2006 Matt Howlett and others.
 * Copyright (C) 2013 Scott Stephens
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification,
 * are permitted provided that the following conditions are met:
 * 
 * 1. Redistributions of source code must retain the above copyright notice, this
 *    list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 *    this list of conditions and the following disclaimer in the documentation
 *    and/or other materials provided with the distribution.
 * 3. Neither the name of Florence nor the names of its contributors may
 *    be used to endorse or promote products derived from this software without
 *    specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE
 * OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
 * OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Florence;

namespace DemoLib.PlotSurface2DDemo
{
    public class PlotSincFunction : IDemo
    {
        public void Cleanup() { }

        public string[] Description
        {
            get
            {
                return new string[] {"Sinc Function Example. Demonstrates - ",
                "  * Charting line and point plot at the same time.",
                "  * Adding a legend." };
            }
        }

        public void CreatePlot(InteractivePlotSurface2D plotSurface)
        {

            plotSurface.Clear(); // clear everything. reset fonts. remove plot components etc.

            System.Random r = new Random();
            double[] a = new double[100];
            double[] b = new double[100];
            double mult = 0.00001f;
            for (int i = 0; i < 100; ++i)
            {
                a[i] = ((double)r.Next(1000) / 5000.0f - 0.1f) * mult;
                if (i == 50) { b[i] = 1.0f * mult; }
                else
                {
                    b[i] = (double)Math.Sin((((double)i - 50.0f) / 4.0f)) / (((double)i - 50.0f) / 4.0f);
                    b[i] *= mult;
                }
                a[i] += b[i];
            }

            Marker m = new Marker(Marker.MarkerType.Cross1, 6, new Pen(Color.Blue, 2.0F));
            PointPlot pp = new PointPlot(m);
            pp.OrdinateData = a;
            pp.AbscissaData = new StartStep(-500.0, 10.0);
            pp.Label = "Random";
            plotSurface.Add(pp);

            LinePlot lp = new LinePlot();
            lp.OrdinateData = b;
            lp.AbscissaData = new StartStep(-500.0, 10.0);
            lp.Pen = new Pen(Color.Red, 2.0f);
            plotSurface.Add(lp);

            plotSurface.Title = "Sinc Function";
            plotSurface.YAxis1.Label = "Magnitude";
            plotSurface.XAxis1.Label = "Position";

            Legend legend = new Legend();
            legend.AttachTo(PlotSurface2D.XAxisPosition.Top, PlotSurface2D.YAxisPosition.Left);
            legend.VerticalEdgePlacement = Legend.Placement.Inside;
            legend.HorizontalEdgePlacement = Legend.Placement.Inside;
            legend.YOffset = 8;

            plotSurface.Legend = legend;
            plotSurface.LegendZOrder = 1; // default zorder for adding idrawables is 0, so this puts legend on top.

            plotSurface.Refresh();
        }
    }
}