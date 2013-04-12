/*
 * Florence - A charting library for .NET
 * 
 * PlotQE.cs
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
using System.Timers;

using Florence;

namespace DemoLib.PlotSurface2DDemo
{
    public class PlotQE : IDemo
    {

        public string[] Description
        {
            get
            {
                return new string[] {
                "Cs2Te Photocathode QE evolution Example. Demonstrates - ",
                "  * LabelPointPlot (allows text to be associated with points)",
                "  * PointPlot droplines",
                "  * LabelAxis",
			    "  * PhysicalSpacingMin property of LabelAxis",
                "",
                "You cannot interact with this chart"};
            }
        }

        private double[] PlotQEExampleValues;
        private string[] PlotQEExampleTextValues;
        private Timer Timer;
        private InteractivePlotSurface2D plotSurface;

        public void CreatePlot(InteractivePlotSurface2D plotSurface)
        {
            this.plotSurface = plotSurface;

            this.Timer = new System.Timers.Timer();
            this.Timer.Interval = 500;
            this.Timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            this.Timer.Enabled = true;
            plotSurface.Clear();

            int len = 24;
            string[] s = new string[len];
            PlotQEExampleValues = new double[len];
            PlotQEExampleTextValues = new string[len];

            Random r = new Random();

            for (int i = 0; i < len; i++)
            {
                PlotQEExampleValues[i] = 8.0f + 12.0f * (double)r.Next(10000) / 10000.0f;
                if (PlotQEExampleValues[i] > 18.0f)
                {
                    PlotQEExampleTextValues[i] = "KCsTe";
                }
                else
                {
                    PlotQEExampleTextValues[i] = "";
                }
                s[i] = i.ToString("00") + ".1";
            }

            PointPlot pp = new PointPlot();
            pp.DataSource = PlotQEExampleValues;
            pp.Marker = new Marker(Marker.MarkerType.Square, 10);
            pp.Marker.DropLine = true;
            pp.Marker.Pen = Pens.CornflowerBlue;
            pp.Marker.Filled = false;
            plotSurface.Add(pp);

            LabelPointPlot tp1 = new LabelPointPlot();
            tp1.DataSource = PlotQEExampleValues;
            tp1.TextData = PlotQEExampleTextValues;
            tp1.LabelTextPosition = LabelPointPlot.LabelPositions.Above;
            tp1.Marker = new Marker(Marker.MarkerType.None, 10);
            plotSurface.Add(tp1);

            LabelAxis la = new LabelAxis(plotSurface.XAxis1);
            for (int i = 0; i < len; ++i)
            {
                la.AddLabel(s[i], i);
            }
            FontFamily ff = new FontFamily("Verdana");
            la.TickTextFont = new Font(ff, 7);
            la.PhysicalSpacingMin = 25;
            plotSurface.XAxis1 = la;

            plotSurface.Title = "Cs2Te Photocathode QE evolution";
            plotSurface.TitleFont = new Font(ff, 15);
            plotSurface.XAxis1.WorldMin = -1.0f;
            plotSurface.XAxis1.WorldMax = len;
            plotSurface.XAxis1.LabelFont = new Font(ff, 10);
            plotSurface.XAxis1.Label = "Cathode ID";
            plotSurface.YAxis1.Label = "QE [%]";
            plotSurface.YAxis1.LabelFont = new Font(ff, 10);
            plotSurface.YAxis1.TickTextFont = new Font(ff, 10);

            plotSurface.YAxis1.WorldMin = 0.0;
            plotSurface.YAxis1.WorldMax = 25.0;

            plotSurface.XAxis1.TicksLabelAngle = 60.0f;

            plotSurface.Refresh();
        }

        /// <summary>
        /// Callback for QE example timer tick.
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {

            Random r = new Random();

            for (int i = 0; i < PlotQEExampleValues.Length; ++i)
            {
                PlotQEExampleValues[i] = 8.0f + 12.0f * (double)r.Next(10000) / 10000.0f;
                if (PlotQEExampleValues[i] > 18.0f)
                {
                    PlotQEExampleTextValues[i] = "KCsTe";
                }
                else
                {
                    PlotQEExampleTextValues[i] = "";
                }
            }

            plotSurface.Refresh();
        }

        public void Cleanup()
        {
            this.Timer.Enabled = false;
            plotSurface = null;
        }
    }
}