/*
 * Florence - A charting library for .NET
 * 
 * PlotGaussian.cs
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
    public class PlotGaussian : IDemo
    {
        public void Cleanup() { }

        public string[] Description
        {
            get
            {
                return new string[] {
                "Gaussian Example. Demonstrates - ",
                "  * HistogramPlot and LinePlot." };
            }
        }

        public void CreatePlot(InteractivePlotSurface2D plotSurface)
        {

            plotSurface.Clear();

            System.Random r = new Random();

            int len = 35;
            double[] a = new double[len];
            double[] b = new double[len];

            for (int i = 0; i < len; ++i)
            {
                int j = len - 1 - i;
                a[i] = (double)Math.Exp(-(double)(i - len / 2) * (double)(i - len / 2) / 50.0f);
                b[i] = a[i] + (r.Next(10) / 50.0f) - 0.05f;
                if (b[i] < 0.0f)
                {
                    b[i] = 0;
                }
            }

            HistogramPlot sp = new HistogramPlot();
            sp.DataSource = b;
            sp.Pen = Pens.DarkBlue;
            sp.Filled = true;
            sp.RectangleBrush = new RectangleBrushes.HorizontalCenterFade(Color.Lavender, Color.Gold);
            sp.BaseWidth = 0.5f;
            sp.Label = "Random Data";
            LinePlot lp = new LinePlot();
            lp.DataSource = a;
            lp.Pen = new Pen(Color.Blue, 3.0f);
            lp.Label = "Gaussian Function";
            plotSurface.Add(sp);
            plotSurface.Add(lp);
            plotSurface.Legend = new Legend();
            plotSurface.YAxis1.WorldMin = 0.0f;
            plotSurface.Title = "Histogram Plot";
            plotSurface.Refresh();
        }
    }
}