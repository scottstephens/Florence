/*
 * Florence - A charting library for .NET
 * 
 * PlotMultiHistogram.cs
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
    public class PlotMultiHistogram : IDemo
    {
        public void Cleanup() { }

        public string[] Description
        {
            get
            {
                return new string[] { };
            }
        }

        public void CreatePlot(InteractivePlotSurface2D plotSurface)
        {
            double[] data = new double[] { 0, 4, 3, 2, 5, 4, 2, 3 };
            double[] data2 = new double[] { 5, 2, 4, 1, 2, 1, 5, 3 };

            HistogramPlot hp = new HistogramPlot();
            hp.OrdinateData = data;
            hp.RectangleBrush = RectangleBrushes.Horizontal.FaintRedFade;
            hp.Filled = true;
            hp.BaseOffset = -0.15;
            hp.BaseWidth = 0.25f;

            HistogramPlot hp2 = new HistogramPlot();
            hp2.OrdinateData = data2;
            hp2.RectangleBrush = RectangleBrushes.Horizontal.FaintGreenFade;
            hp2.Filled = true;
            hp2.BaseOffset = 0.15;
            hp2.BaseWidth = 0.25f;

            plotSurface.Clear();

            plotSurface.Add(hp);
            plotSurface.Add(hp2);

            plotSurface.PlotBackBrush = RectangleBrushes.Vertical.FaintBlueFade;
            plotSurface.Refresh();
        }
    }
}