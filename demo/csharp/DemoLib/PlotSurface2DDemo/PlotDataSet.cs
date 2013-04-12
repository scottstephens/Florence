/*
 * Florence - A charting library for .NET
 * 
 * PlotDataSet.cs
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
using System.Data;
using System.Reflection;
using System.Collections;

namespace DemoLib.PlotSurface2DDemo
{
    public class PlotDataSet : IDemo
    {
        public void Cleanup() { }

        public string[] Description
        {
            get
            {
                return new string[] {
                "Stock Data Example. Demonstrates - ",
                "  * CandlePlot, FilledRegion, LinePlot and ArrowItem IDrawables",
                "  * DateTime axes",
                "  * A few plot interactions. Try (a) dragging the axes (b) dragging the plot surface."};
            }
        }

        public void CreatePlot(InteractivePlotSurface2D plotSurface)
        {

            plotSurface.Clear();
            //plotSurface.DateTimeToolTip = true;

            // obtain stock information from xml file
            DataSet ds = new DataSet();
            System.IO.Stream file =
                Assembly.GetExecutingAssembly().GetManifestResourceStream("DemoLib.Resources.asx_jbh.xml");
            ds.ReadXml(file, System.Data.XmlReadMode.ReadSchema);
            DataTable dt = ds.Tables[0];
            DataView dv = new DataView(dt);

            // create CandlePlot.
            CandlePlot cp = new CandlePlot();
            cp.DataSource = dt;
            cp.AbscissaData = "Date";
            cp.OpenData = "Open";
            cp.LowData = "Low";
            cp.HighData = "High";
            cp.CloseData = "Close";
            cp.BearishColor = Color.Red;
            cp.BullishColor = Color.Green;
            cp.Style = CandlePlot.Styles.Filled;

            // calculate 10 day moving average and 2*sd line
            ArrayList av10 = new ArrayList();
            ArrayList sd2_10 = new ArrayList();
            ArrayList sd_2_10 = new ArrayList();
            ArrayList dates = new ArrayList();
            for (int i = 0; i < dt.Rows.Count - 10; ++i)
            {
                float sum = 0.0f;
                for (int j = 0; j < 10; ++j)
                {
                    sum += (float)dt.Rows[i + j]["Close"];
                }
                float average = sum / 10.0f;
                av10.Add(average);
                sum = 0.0f;
                for (int j = 0; j < 10; ++j)
                {
                    sum += ((float)dt.Rows[i + j]["Close"] - average) * ((float)dt.Rows[i + j]["Close"] - average);
                }
                sum /= 10.0f;
                sum = 2.0f * (float)Math.Sqrt(sum);
                sd2_10.Add(average + sum);
                sd_2_10.Add(average - sum);
                dates.Add((DateTime)dt.Rows[i + 10]["Date"]);
            }

            // and a line plot of close values.
            LinePlot av = new LinePlot();
            av.OrdinateData = av10;
            av.AbscissaData = dates;
            av.Color = Color.LightGray;
            av.Pen.Width = 2.0f;

            LinePlot top = new LinePlot();
            top.OrdinateData = sd2_10;
            top.AbscissaData = dates;
            top.Color = Color.LightSteelBlue;
            top.Pen.Width = 2.0f;

            LinePlot bottom = new LinePlot();
            bottom.OrdinateData = sd_2_10;
            bottom.AbscissaData = dates;
            bottom.Color = Color.LightSteelBlue;
            bottom.Pen.Width = 2.0f;

            FilledRegion fr = new FilledRegion(top, bottom);
            //fr.RectangleBrush = new RectangleBrushes.Vertical( Color.FloralWhite, Color.GhostWhite );
            fr.RectangleBrush = new RectangleBrushes.Vertical(Color.FromArgb(255, 255, 240), Color.FromArgb(240, 255, 255));
            plotSurface.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            plotSurface.Add(fr);

            plotSurface.Add(new Grid());

            plotSurface.Add(av);
            plotSurface.Add(top);
            plotSurface.Add(bottom);
            plotSurface.Add(cp);

            // now make an arrow... 
            ArrowItem arrow = new ArrowItem(new PointD(((DateTime)dt.Rows[60]["Date"]).Ticks, 2.28), -80, "An interesting flat bit");
            arrow.ArrowColor = Color.DarkBlue;
            arrow.PhysicalLength = 50;

            //plotSurface.Add( arrow );

            plotSurface.Title = "AU:JBH";
            plotSurface.XAxis1.Label = "Date / Time";
            plotSurface.XAxis1.WorldMin += plotSurface.XAxis1.WorldLength / 4.0;
            plotSurface.XAxis1.WorldMax -= plotSurface.XAxis1.WorldLength / 2.0;
            plotSurface.YAxis1.Label = "Price [$]";

            plotSurface.XAxis1 = new TradingDateTimeAxis(plotSurface.XAxis1);

            plotSurface.AddInteraction(new PlotDrag(true, true));
            plotSurface.AddInteraction(new AxisDrag());


            // make sure plot surface colors are as we expect - the wave example changes them.
            plotSurface.PlotBackColor = Color.White;
            plotSurface.XAxis1.Color = Color.Black;
            plotSurface.YAxis1.Color = Color.Black;

            plotSurface.Refresh();
        }
    }
}