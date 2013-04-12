/*
 * Florence - A charting library for .NET
 * 
 * PlotABC.cs
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
using System.Reflection;

namespace DemoLib.PlotSurface2DDemo
{
    public class PlotABC : IDemo
    {
        public void Cleanup() { }

        public string[] Description
        {
            get
            {
                return new string[] {"ABC (logo for australian broadcasting commission) Example. Demonstrates - ",
                "  * How to set the background of a plotsurface as an image.",
                "  * EqualAspectRatio axis constraint" };
            }
        }

        public void CreatePlot(InteractivePlotSurface2D plotSurface)
        {
            plotSurface.Clear();
			const int size = 200;
			float [] xs = new float [size];
			float [] ys = new float [size];
			for (int i=0; i<size; i++)
			{
				xs[i] = (float)Math.Sin((double)i/(double)(size-1)*2.0*Math.PI);
				ys[i] = (float)Math.Cos((double)i/(double)(size-1)*6.0*Math.PI);
			}

			LinePlot lp = new LinePlot();
			lp.OrdinateData = ys;
			lp.AbscissaData = xs;
			Pen linePen = new Pen( Color.Yellow, 5.0f );
			lp.Pen = linePen;
			plotSurface.Add(lp);
			plotSurface.Title = "AxisConstraint.EqualScaling in action...";

			// Image downloaded from http://squidfingers.com. Thanks!
			Assembly a = Assembly.GetExecutingAssembly();
			System.IO.Stream file =
				a.GetManifestResourceStream( "DemoLib.Resources.pattern01.jpg" );
			System.Drawing.Image im = Image.FromStream( file );
			plotSurface.PlotBackImage = new Bitmap( im );

			plotSurface.AddAxesConstraint( new AxesConstraint.AspectRatio( 1.0, PlotSurface2D.XAxisPosition.Top, PlotSurface2D.YAxisPosition.Left ) );
			plotSurface.XAxis1.WorldMin = plotSurface.YAxis1.WorldMin;
			plotSurface.XAxis1.WorldMax = plotSurface.YAxis1.WorldMax;
			plotSurface.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            plotSurface.AddInteraction(new PlotZoom());

            // make sure plot surface colors are as we expect - the wave example changes them.
            //plotSurface.PlotBackColor = Color.White;
            plotSurface.XAxis1.Color = Color.Black;
            plotSurface.YAxis1.Color = Color.Black;

            plotSurface.Refresh();
        }
    }
}