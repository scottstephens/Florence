/*
 * Florence - A charting library for .NET
 * 
 * MultiPlotDemo.cs
 * Copyright (C) 2003-2006 Matt Howlett and others.
 * Copyright (C) 2003-2013 Hywel Thomas
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using Gtk;
using System.Data;
using System.Reflection;

using Florence;
using Florence.GtkSharp;
using DemoLib.MultiPlotDemo;

namespace FlorenceDemoGtkSharp
{
    /// <summary>
    /// Summary description for Financial Demo.
    /// </summary>
    public class FinancialDemo : Gtk.Window
    {
        private PlotWidget volumeWidget;
        private PlotWidget costWidget;
        private DemoLib.MultiPlotDemo.FinancialDemo Demo;

        private Gtk.Button closeButton;

        public FinancialDemo()
            : base("Multiple linked plot demo")
        {
            //
            // Gtk Window Setup
            //
            InitializeComponent();

            this.Demo = new DemoLib.MultiPlotDemo.FinancialDemo();
            this.Demo.CreatePlot();

            this.costWidget.InteractivePlotSurface2D = this.Demo.costPS;
            this.volumeWidget.InteractivePlotSurface2D = this.Demo.volumePS;
        }

        #region Gtk Window Setup code

        private void InitializeComponent()
        {
            //
            // costWidget
            //
            this.costWidget = new PlotWidget();
            //
            // volumeWidget
            //
            this.volumeWidget = new PlotWidget();
            // 
            // closeButton
            // 
            this.closeButton = new Gtk.Button("Close");
            this.closeButton.Clicked += new System.EventHandler(this.closeButton_Click);

            // 
            // FinancialDemo
            // 
            this.SetSizeRequest(630, 450);
            //
            // Define a 10x10 table on which to lay out the plots and button
            //
            Gtk.Table layout = new Gtk.Table(10, 10, true);
            layout.BorderWidth = 4;
            Add(layout);

            AttachOptions opt = AttachOptions.Expand | AttachOptions.Fill;
            uint xpad = 2, ypad = 10;

            layout.Attach(costWidget, 0, 10, 0, 6);
            layout.Attach(volumeWidget, 0, 10, 6, 9);
            layout.Attach(closeButton, 1, 2, 9, 10, opt, opt, xpad, ypad);
            this.Name = "PlotSurface2DDemo";

            this.Name = "FinancialDemo";

        }
        #endregion


        /// <summary>
        /// Callback for close button.
        /// </summary>
        private void closeButton_Click(object sender, System.EventArgs e)
        {
            this.Destroy();
        }


    }
}

