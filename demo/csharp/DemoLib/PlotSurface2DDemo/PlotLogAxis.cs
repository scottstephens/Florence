/*
 * Florence - A charting library for .NET
 * 
 * PlotLogAxis.cs
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
    public class PlotLogAxis : IDemo
    {

        public void Cleanup() { }

        public string[] Description
        {
            get
            {
                return new string[] {
                "Log Example. Demonstrates - ",
                "  * How to chart data against log axes and linear axes at the same time."};
            }
        }

        public void CreatePlot(InteractivePlotSurface2D plotSurface)
        {
            plotSurface.Clear();

            // draw a fine grid. 
            Grid fineGrid = new Grid();
            fineGrid.VerticalGridType = Grid.GridType.Fine;
            fineGrid.HorizontalGridType = Grid.GridType.Fine;
            plotSurface.Add(fineGrid);

            const int npt = 101;
            float[] x = new float[npt];
            float[] y = new float[npt];
            float step = 0.1f;
            for (int i = 0; i < npt; ++i)
            {
                x[i] = i * step - 5.0f;
                y[i] = (float)Math.Pow(10.0, x[i]);
            }
            float xmin = x[0];
            float xmax = x[npt - 1];
            float ymin = (float)Math.Pow(10.0, xmin);
            float ymax = (float)Math.Pow(10.0, xmax);

            LinePlot lp = new LinePlot();
            lp.OrdinateData = y;
            lp.AbscissaData = x;
            lp.Pen = new Pen(Color.Red);
            plotSurface.Add(lp);

            LogAxis loga = new LogAxis(plotSurface.YAxis1);
            loga.WorldMin = ymin;
            loga.WorldMax = ymax;
            loga.AxisColor = Color.Red;
            loga.LabelColor = Color.Red;
            loga.TickTextColor = Color.Red;
            loga.LargeTickStep = 1.0f;
            loga.Label = "10^x";
            plotSurface.YAxis1 = loga;

            LinePlot lp1 = new LinePlot();
            lp1.OrdinateData = y;
            lp1.AbscissaData = x;
            lp1.Pen = new Pen(Color.Blue);
            plotSurface.Add(lp1, PlotSurface2D.XAxisPosition.Bottom, PlotSurface2D.YAxisPosition.Right);
            LinearAxis lin = new LinearAxis(plotSurface.YAxis2);
            lin.WorldMin = ymin;
            lin.WorldMax = ymax;
            lin.AxisColor = Color.Blue;
            lin.LabelColor = Color.Blue;
            lin.TickTextColor = Color.Blue;
            lin.Label = "10^x";
            plotSurface.YAxis2 = lin;

            LinearAxis lx = (LinearAxis)plotSurface.XAxis1;
            lx.WorldMin = xmin;
            lx.WorldMax = xmax;
            lx.Label = "x";

            //((LogAxis)plotSurface.YAxis1).LargeTickStep = 2;

            plotSurface.Title = "Mixed Linear/Log Axes";

            //plotSurface.XAxis1.LabelOffset = 20.0f;

            plotSurface.Refresh();
        }
    }
}