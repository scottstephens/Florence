/*
 * Florence - A charting library for .NET
 * 
 * FinancialDemo.cs
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

using Florence;
using System.Data;
using System.Reflection;
using System.Drawing;

namespace DemoLib.MultiPlotDemo
{
    public class FinancialDemo
    {
        public InteractivePlotSurface2D volumePS;
        public InteractivePlotSurface2D costPS;

        public FinancialDemo()
        {
            this.volumePS = new InteractivePlotSurface2D();
            this.costPS = new InteractivePlotSurface2D();
        }

        public void CreatePlot()
        {
            costPS.Clear();
            //costPS.DateTimeToolTip = true;

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
            costPS.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            costPS.Add(new Grid());
            costPS.Add(cp);
            costPS.Title = "AU:JBH";
            costPS.YAxis1.Label = "Price [$]";
            costPS.YAxis1.LabelOffset = 40;
            costPS.YAxis1.LabelOffsetAbsolute = true;
            costPS.XAxis1.HideTickText = true;
            costPS.SurfacePadding = 5;
            costPS.AddInteraction(new PlotDrag(true, true));
            costPS.AddInteraction(new AxisDrag());
            costPS.InteractionOccurred += costPS_InteractionOccured;
            costPS.AddAxesConstraint(new AxesConstraint.AxisPosition(PlotSurface2D.YAxisPosition.Left, 60));

            PointPlot pp = new PointPlot();
            pp.Marker = new Marker(Marker.MarkerType.Square, 0);
            pp.Marker.Pen = new Pen(Color.Red, 5.0f);
            pp.Marker.DropLine = true;
            pp.DataSource = dt;
            pp.AbscissaData = "Date";
            pp.OrdinateData = "Volume";
            volumePS.Add(pp);
            volumePS.YAxis1.Label = "Volume";
            volumePS.YAxis1.LabelOffsetAbsolute = true;
            volumePS.YAxis1.LabelOffset = 40;
            volumePS.SurfacePadding = 5;
            volumePS.AddAxesConstraint(new AxesConstraint.AxisPosition(PlotSurface2D.YAxisPosition.Left, 60));
            volumePS.AddInteraction(new AxisDrag());
            volumePS.AddInteraction(new PlotDrag(true, false));
            volumePS.InteractionOccurred += volumePS_InteractionOccured;
            volumePS.PreRefresh += volumePS_PreRefresh;

            //this.costPS.RightMenu = new ReducedContextMenu();
        }

        /// <summary>
        /// When the costPS chart has changed, this is called which updates the volumePS chart.
        /// </summary>
        /// <param name="sender"></param>
        private void costPS_InteractionOccured(object sender)
        {
            DateTimeAxis axis = new DateTimeAxis(costPS.XAxis1);
            axis.Label = "Date / Time";
            axis.HideTickText = false;
            this.volumePS.XAxis1 = axis;
            this.volumePS.Refresh();
        }


        /// <summary>
        /// When the volumePS chart has changed, this is called which updates hte costPS chart.
        /// </summary>
        private void volumePS_InteractionOccured(object sender)
        {
            DateTimeAxis axis = new DateTimeAxis(volumePS.XAxis1);
            axis.Label = "";
            axis.HideTickText = true;
            this.costPS.XAxis1 = axis;
            this.costPS.Refresh();
        }

        /// <summary>
        /// This is called prior to volumePS refresh to enforce the WorldMin is 0. 
        /// This may have been changed by the axisdrag interaction.
        /// </summary>
        /// <param name="sender"></param>
        private void volumePS_PreRefresh(object sender)
        {
            volumePS.YAxis1.WorldMin = 0.0;
        }
    }
}
