/*
 * Florence - A charting library for .NET
 * 
 * PlotCircular.cs
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
    public class PlotCircular : IDemo
    {
        public string[] Description 
        { 
            get 
            {
                return new string[]{
                "Circular Example. Demonstrates - ",
                "  * PiAxis, Horizontal and Vertical Lines.",
                "  * Placement of legend" };
            }   
        }

        public void CreatePlot(InteractivePlotSurface2D plotSurface)
        {
            plotSurface.Clear();
            plotSurface.Add(new HorizontalLine(0.0, Color.LightGray));
            plotSurface.Add(new VerticalLine(0.0, Color.LightGray));

            const int N = 400;
            const double start = -Math.PI * 7.0;
            const double end = Math.PI * 7.0;

            double[] xs = new double[N];
            double[] ys = new double[N];

            for (int i = 0; i < N; ++i)
            {
                double t = ((double)i * (end - start) / (double)N + start);
                xs[i] = 0.5 * (t - 2.0 * Math.Sin(t));
                ys[i] = 2.0 * (1.0 - 2.0 * Math.Cos(t));
            }

            LinePlot lp = new LinePlot(ys, xs);
            lp.Pen = new Pen(Color.DarkBlue, 2.0f);
            lp.Label = "Circular Line"; // no legend, but still useful for copy data to clipboard.
            plotSurface.Add(lp);

            plotSurface.XAxis1 = new PiAxis(plotSurface.XAxis1);

            plotSurface.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            plotSurface.Legend = new Legend();
            plotSurface.Legend.AttachTo(PlotSurface2D.XAxisPosition.Bottom, PlotSurface2D.YAxisPosition.Right);
            plotSurface.Legend.HorizontalEdgePlacement = Legend.Placement.Inside;
            plotSurface.Legend.VerticalEdgePlacement = Legend.Placement.Inside;
            plotSurface.Legend.XOffset = -10;
            plotSurface.Legend.YOffset = -10;

            plotSurface.Refresh();
        }


        public void Cleanup() { }
        
    }
}
