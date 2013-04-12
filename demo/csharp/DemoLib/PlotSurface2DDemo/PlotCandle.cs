/*
 * Florence - A charting library for .NET
 * 
 * PlotCandle.cs
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

namespace DemoLib.PlotSurface2DDemo
{
    public class PlotCandle : IDemo
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
            plotSurface.Clear();

            // obtain stock information from xml file
            DataSet ds = new DataSet();
            System.IO.Stream file =
                Assembly.GetExecutingAssembly().GetManifestResourceStream("DemoLib.Resources.asx_jbh.xml");
            ds.ReadXml(file, System.Data.XmlReadMode.ReadSchema);
            DataTable dt = ds.Tables[0];

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
            cp.StickWidth = 3;
            cp.Color = Color.DarkBlue;

            plotSurface.Add(new Grid());
            plotSurface.Add(cp);

            plotSurface.Title = "AU:JBH";
            plotSurface.XAxis1.Label = "Date / Time";
            plotSurface.YAxis1.Label = "Price [$]";

            plotSurface.Refresh();
        }
    }
}