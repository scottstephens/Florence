/*
 * Florence - A charting library for .NET
 * 
 * PlotLabelAxis.cs
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
    public class PlotLabelAxis : IDemo
    {
        public void Cleanup() { }

        public string[] Description
        {
            get
            {
                return new string[] {"Internet Usage Example. Demonstrates - ",
                "  * Label Axis with angular text.",
                "  * RectangleBrushes." };

            }
        }

        public void CreatePlot(InteractivePlotSurface2D plotSurface)
        {
            
            plotSurface.Clear();

			Grid mygrid = new Grid();
			mygrid.VerticalGridType = Grid.GridType.Coarse;
			Pen majorGridPen = new Pen( Color.LightGray );
			float[] pattern = { 1.0f, 2.0f };
			majorGridPen.DashPattern = pattern;
			mygrid.MajorGridPen = majorGridPen;
			plotSurface.Add( mygrid );

			float[] xs = {20.0f, 31.0f, 27.0f, 38.0f, 24.0f, 3.0f, 2.0f };
			float[] xs2 = {7.0f, 10.0f, 42.0f, 9.0f, 2.0f, 79.0f, 70.0f };
			float[] xs3 = {1.0f, 20.0f, 20.0f, 25.0f, 10.0f, 30.0f, 30.0f };

			HistogramPlot hp = new HistogramPlot();
			hp.DataSource = xs;
			hp.BaseWidth = 0.6f;
			hp.RectangleBrush =
				new RectangleBrushes.HorizontalCenterFade( Color.FromArgb(255,255,200), Color.White );
			hp.Filled = true;
			hp.Label = "Developer Work";
			
            HistogramPlot hp2 = new HistogramPlot();
			hp2.DataSource = xs2;
			hp2.Label = "Web Browsing";
			hp2.RectangleBrush = RectangleBrushes.Horizontal.FaintGreenFade;
			hp2.Filled = true;
			hp2.StackedTo( hp );
			
            HistogramPlot hp3 = new HistogramPlot();
			hp3.DataSource = xs3;
			hp3.Label = "P2P Downloads";
			hp3.RectangleBrush = RectangleBrushes.Vertical.FaintBlueFade;
			hp3.Filled = true;
			hp3.StackedTo( hp2 );
			
            plotSurface.Add( hp );
			plotSurface.Add( hp2 );
			plotSurface.Add( hp3 );
			
            plotSurface.Legend = new Legend();

			LabelAxis la = new LabelAxis( plotSurface.XAxis1 );
			la.AddLabel( "Monday", 0.0f );
			la.AddLabel( "Tuesday", 1.0f );
			la.AddLabel( "Wednesday", 2.0f );
			la.AddLabel( "Thursday", 3.0f );
			la.AddLabel( "Friday", 4.0f );
			la.AddLabel( "Saturday", 5.0f );
			la.AddLabel( "Sunday", 6.0f );
			la.Label = "Days";
			la.TickTextFont = new Font( "Courier New", 8 );
			la.TicksBetweenText = true;

			plotSurface.XAxis1 = la;
			plotSurface.YAxis1.WorldMin = 0.0;
			plotSurface.YAxis1.Label = "MBytes";
			((LinearAxis)plotSurface.YAxis1).NumberOfSmallTicks = 1;

			plotSurface.Title = "Internet useage for user:\n johnc 09/01/03 - 09/07/03";

			plotSurface.XAxis1.TicksLabelAngle = 30.0f;
                        
			plotSurface.PlotBackBrush = RectangleBrushes.Vertical.FaintRedFade;
			plotSurface.Refresh();
        }
    }
}