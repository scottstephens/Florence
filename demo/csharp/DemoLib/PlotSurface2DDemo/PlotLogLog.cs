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
    public class PlotLogLog : IDemo
    {
        public void Cleanup() { }

        public string[] Description
        {
            get
            {
                return new string[] {"LogLog Example. Demonstrates - ",
                "  * How to chart data against log axes and linear axes at the same time."};
            }
        }

        public void CreatePlot(InteractivePlotSurface2D plotSurface)
        {
            
            // log log plot
			plotSurface.Clear();

			Grid mygrid = new Grid();
			mygrid.HorizontalGridType = Grid.GridType.Fine;
			mygrid.VerticalGridType = Grid.GridType.Fine;
			plotSurface.Add(mygrid);

			int npt = 101;
			float [] x = new float[npt];
			float [] y = new float[npt];

			float step=0.1f;

			// plot a power law on the log-log scale
			for (int i=0; i<npt; ++i)
			{
				x[i] = (i+1)*step;
				y[i] = x[i]*x[i];
			}
			float xmin = x[0];
			float xmax = x[npt-1];
			float ymin = y[0];
			float ymax = y[npt-1];

			LinePlot lp = new LinePlot();
			lp.OrdinateData = y;
			lp.AbscissaData = x; 
			lp.Pen = new Pen( Color.Red );
			plotSurface.Add( lp );
			// axes
			// x axis
			LogAxis logax = new LogAxis( plotSurface.XAxis1 );
			logax.WorldMin = xmin;
			logax.WorldMax = xmax;
			logax.AxisColor = Color.Red;
			logax.LabelColor = Color.Red;
			logax.TickTextColor = Color.Red;
			logax.LargeTickStep = 1.0f;
			logax.Label = "x";
			plotSurface.XAxis1 = logax;
			// y axis
			LogAxis logay = new LogAxis( plotSurface.YAxis1 );
			logay.WorldMin = ymin;
			logay.WorldMax = ymax;
			logay.AxisColor = Color.Red;
			logay.LabelColor = Color.Red;
			logay.TickTextColor = Color.Red;
			logay.LargeTickStep = 1.0f;
			logay.Label = "x^2";
			plotSurface.YAxis1 = logay;

			LinePlot lp1 = new LinePlot();
			lp1.OrdinateData = y;
			lp1.AbscissaData = x;
			lp1.Pen = new Pen( Color.Blue );
			plotSurface.Add( lp1, PlotSurface2D.XAxisPosition.Top, PlotSurface2D.YAxisPosition.Right );
			// axes
			// x axis (lin)
			LinearAxis linx = (LinearAxis) plotSurface.XAxis2;
			linx.WorldMin = xmin;
			linx.WorldMax = xmax;
			linx.AxisColor = Color.Blue;
			linx.LabelColor = Color.Blue;
			linx.TickTextColor = Color.Blue;
			linx.Label = "x";
			plotSurface.XAxis2 = linx;
			// y axis (lin)
			LinearAxis liny = (LinearAxis) plotSurface.YAxis2;
			liny.WorldMin = ymin;
			liny.WorldMax = ymax;
			liny.AxisColor = Color.Blue;
			liny.LabelColor = Color.Blue;
			liny.TickTextColor = Color.Blue;
			liny.Label = "x^2";
			plotSurface.YAxis2 = liny;

			plotSurface.Title = "x^2 plotted with log(red)/linear(blue) axes";

			plotSurface.Refresh();
        }
    }
}