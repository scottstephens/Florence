/*
 * Florence - A charting library for .NET
 * 
 * PlotMockup.cs
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

namespace DemoLib.PlotSurface2DDemo
{
    public class PlotMockup : IDemo
    {
        public void Cleanup() { }

        public string[] Description
        {
            get
            {
                return new string[] {
								 "THE TEST (can your charting library handle this?) - ",
								 "Florence demonstrates it can handle real world charting requirements." };
            }
        }

        public void CreatePlot(InteractivePlotSurface2D plotSurface)
        {

            // first of all, generate some mockup data.
            DataTable info = new DataTable("Store Information");
            info.Columns.Add("Index", typeof(int));
            info.Columns.Add("IndexOffsetLeft", typeof(float));
            info.Columns.Add("IndexOffsetRight", typeof(float));
            info.Columns.Add("StoreName", typeof(string));
            info.Columns.Add("BarBase", typeof(float));
            info.Columns.Add("StoreGrowth", typeof(float));
            info.Columns.Add("AverageGrowth", typeof(float));
            info.Columns.Add("ProjectedSales", typeof(float));

            float barBase = 185.0f;
            Random r = new Random();
            for (int i = 0; i < 18; ++i)
            {
                DataRow row = info.NewRow();
                row["Index"] = i;
                row["IndexOffsetLeft"] = (float)i - 0.1f;
                row["IndexOffsetRight"] = (float)i + 0.1f;
                row["StoreName"] = "Store " + (i + 1).ToString();
                row["BarBase"] = barBase;
                row["StoreGrowth"] = barBase + ((r.NextDouble() - 0.1) * 20.0f);
                row["AverageGrowth"] = barBase + ((r.NextDouble() - 0.1) * 15.0f);
                row["ProjectedSales"] = barBase + (r.NextDouble() * 15.0f);
                info.Rows.Add(row);
                barBase += (float)r.NextDouble() * 4.0f;
            }


            plotSurface.Clear();

            plotSurface.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // generate the grid 
            Grid grid = new Grid();
            grid.VerticalGridType = Grid.GridType.Coarse;
            grid.HorizontalGridType = Grid.GridType.None;
            grid.MajorGridPen = new Pen(Color.Black, 1.0f);
            plotSurface.Add(grid);

            // generate the trendline 
            LinePlot trendline = new LinePlot();
            trendline.DataSource = info;
            trendline.AbscissaData = "Index";
            trendline.OrdinateData = "BarBase";
            trendline.Pen = new Pen(Color.Black, 3.0f);
            trendline.Label = "Trendline";
            plotSurface.Add(trendline);

            // draw store growth bars
            BarPlot storeGrowth = new BarPlot();
            storeGrowth.DataSource = info;
            storeGrowth.AbscissaData = "IndexOffsetLeft";
            storeGrowth.OrdinateDataTop = "StoreGrowth";
            storeGrowth.OrdinateDataBottom = "BarBase";
            storeGrowth.Label = "Store Growth";
            storeGrowth.FillBrush = Florence.RectangleBrushes.Solid.Black;
            //storeGrowth.BorderPen = new Pen( Color.Black, 2.0f );
            plotSurface.Add(storeGrowth);

            // draw average growth bars
            BarPlot averageGrowth = new BarPlot();
            averageGrowth.DataSource = info;
            averageGrowth.AbscissaData = "IndexOffsetRight";
            averageGrowth.OrdinateDataBottom = "BarBase";
            averageGrowth.OrdinateDataTop = "AverageGrowth";
            averageGrowth.Label = "Average Growth";
            averageGrowth.FillBrush = Florence.RectangleBrushes.Solid.Gray;
            //averageGrowth.BorderPen = new Pen( Color.Black, 2.0f );
            plotSurface.Add(averageGrowth);

            // generate the projected sales step line.
            StepPlot projected = new StepPlot();
            projected.DataSource = info;
            projected.AbscissaData = "Index";
            projected.OrdinateData = "ProjectedSales";
            projected.Pen = new Pen(Color.Orange, 3.0f);
            projected.HideVerticalSegments = true;
            projected.Center = true;
            projected.Label = "Projected Sales";
            projected.WidthScale = 0.7f;
            plotSurface.Add(projected);

            // generate the minimum target line.
            HorizontalLine minimumTargetLine = new HorizontalLine(218, new Pen(Color.Green, 3.5f));
            minimumTargetLine.Label = "Minimum Target";
            minimumTargetLine.LengthScale = 0.98f;
            minimumTargetLine.ShowInLegend = true; // off by default for lines.
            plotSurface.Add(minimumTargetLine);

            // generate the preferred target line.
            HorizontalLine preferredTargetLine = new HorizontalLine(228, new Pen(Color.Blue, 3.5f));
            preferredTargetLine.Label = "Preferred Target";
            preferredTargetLine.LengthScale = 0.98f;
            preferredTargetLine.ShowInLegend = true; // off by default for lines.
            plotSurface.Add(preferredTargetLine);

            // make some modifications so that chart matches requirements.
            // y axis.
            plotSurface.YAxis1.TicksIndependentOfPhysicalExtent = true;
            plotSurface.YAxis1.TickTextNextToAxis = false;
            plotSurface.YAxis1.TicksAngle = 3.0f * (float)Math.PI / 2.0f;
            ((LinearAxis)plotSurface.YAxis1).LargeTickStep = 10.0;
            ((LinearAxis)plotSurface.YAxis1).NumberOfSmallTicks = 0;

            // x axis
            plotSurface.XAxis1.TicksIndependentOfPhysicalExtent = true;
            plotSurface.XAxis1.TickTextNextToAxis = false;
            plotSurface.XAxis1.TicksAngle = (float)Math.PI / 2.0f;
            LabelAxis la = new LabelAxis(plotSurface.XAxis1);
            for (int i = 0; i < info.Rows.Count; ++i)
            {
                la.AddLabel((string)info.Rows[i]["StoreName"], Convert.ToInt32(info.Rows[i]["Index"]));
            }
            la.TicksLabelAngle = (float)90.0f;
            la.TicksBetweenText = true;
            plotSurface.XAxis1 = la;

            plotSurface.XAxis2 = (Axis)plotSurface.XAxis1.Clone();
            plotSurface.XAxis2.HideTickText = true;
            plotSurface.XAxis2.LargeTickSize = 0;

            Legend l = new Legend();
            l.NumberItemsVertically = 2;
            l.AttachTo(Florence.PlotSurface2D.XAxisPosition.Bottom, Florence.PlotSurface2D.YAxisPosition.Left);
            l.HorizontalEdgePlacement = Florence.Legend.Placement.Outside;
            l.VerticalEdgePlacement = Florence.Legend.Placement.Inside;
            l.XOffset = 5;
            l.YOffset = 50;
            l.BorderStyle = Florence.LegendBase.BorderType.Line;

            plotSurface.Legend = l;

            plotSurface.Title =
                "Sales Growth Compared to\n" +
                "Average Sales Growth by Store Size - Rank Order Low to High";

            plotSurface.Refresh();
        }
    }
}