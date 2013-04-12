/*
 * Florence - A charting library for .NET
 * 
 * PlotWave.cs
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
    public class PlotWave : IDemo
    {
        public void Cleanup() { }

        public string[] Description
        {
            get
            {
                return new string[] {
                "Sound Wave Example. Demonstrates - ",
                "  * StepPlot (centered) and HorizontalLine IDrawables",
                "  * How to set colors of various things.",
                "  * A few plot interactions. Try left clicking and dragging (a) the axes (b) in the plot region.",
                "  * In the future I plan add a plot interaction for axis drag that knows if the ctr key is down. This will select between drag/scale" };
            
            }
        }

        public void CreatePlot(InteractivePlotSurface2D plotSurface)
        {

            //FileStream fs = new FileStream( @"c:\light.wav", System.IO.FileMode.Open );
            System.IO.Stream file =
                Assembly.GetExecutingAssembly().GetManifestResourceStream("DemoLib.Resources.light.wav");


            System.Int16[] w = new short[5000];
            byte[] a = new byte[10000];
            file.Read(a, 0, 10000);
            for (int i = 100; i < 5000; ++i)
            {
                w[i] = BitConverter.ToInt16(a, i * 2);
            }

            file.Close();

            plotSurface.Clear();
            plotSurface.AddInteraction(new VerticalGuideline(Color.Gray));
            plotSurface.AddInteraction(new HorizontalGuideline(Color.Gray));
            plotSurface.AddInteraction(new PlotDrag(true, true));
            plotSurface.AddInteraction(new AxisDrag());

            plotSurface.Add(new HorizontalLine(0.0, Color.LightBlue));

            StepPlot sp = new StepPlot();
            sp.DataSource = w;
            sp.Color = Color.Yellow;
            sp.Center = true;
            plotSurface.Add(sp);

            plotSurface.YAxis1.FlipTicksLabel = true;

            plotSurface.OuterBackColor = Color.Black;
            plotSurface.PlotBackColor = Color.DarkBlue;
            plotSurface.XAxis1.Color = Color.White;
            plotSurface.YAxis1.Color = Color.White;

            plotSurface.Refresh();
        }
    }
}