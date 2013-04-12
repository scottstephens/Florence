/*
 * Florence - A charting library for .NET
 * 
 * PlotSurface2DDemo.cs
 * Copyright (C) 2003-2006 Matt Howlett and others.
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
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Data;
using Florence;
using System.IO;
using System.Reflection;

using DemoLib.PlotSurface2DDemo;

namespace FlorenceDemo
{

	/// <summary>
	/// The main demo window.
	/// </summary>
	public class PlotSurface2DDemo : System.Windows.Forms.Form
	{

		/// <summary>
		/// used to keep track of the current demo plot being displayed.
		/// </summary>
		private int currentPlot = 0;

		/// <summary>
		///  list of the plot demos, initialized in the form constructor.
		/// </summary>
		private IDemo [] PlotRoutines;

		// Note that a Florence.Windows.PlotSurface2D class
		// is used here. This has exactly the same 
		// functionality as the Florence.PlotSurface2D 
		// class, except that it is derived from Forms.UserControl
		// and automatically paints itself in a windows.forms
		// application. Windows.PlotSurface2D can also paint itself
		// to other arbitrary Drawing.Graphics drawing surfaces
		// using the Draw method. (see printing later).

		private System.Windows.Forms.Button quitButton;
        private System.Windows.Forms.Button nextPlotButton;
        private System.Windows.Forms.Button printButton;
		private System.ComponentModel.IContainer components;
		private PrintDocument printDocument;
		private System.Windows.Forms.Button prevPlotButton;
		private Florence.InteractivePlotSurface2D plotSurface;
        private Florence.WinForms.PlotControl plotControl;
		
		private System.Windows.Forms.Label exampleNumberLabel;

        
        private TextBox infoBox;

        private void SetPlot(int plot_number)
        {
            if (currentPlot > 0)
                PlotRoutines[currentPlot].Cleanup();
            currentPlot = plot_number;
            PlotRoutines[currentPlot].CreatePlot(plotSurface);
            infoBox.Lines = PlotRoutines[currentPlot].Description;
            int id = currentPlot + 1;
            exampleNumberLabel.Text = "Plot " + id.ToString("0") + "/" + PlotRoutines.Length.ToString("0");
        }

        public PlotSurface2DDemo()
		{

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            this.plotControl.Anchor = 
				System.Windows.Forms.AnchorStyles.Left |
				System.Windows.Forms.AnchorStyles.Right |
				System.Windows.Forms.AnchorStyles.Top |
				System.Windows.Forms.AnchorStyles.Bottom;

            this.plotSurface = new InteractivePlotSurface2D();
            this.plotControl.InteractivePlotSurface2D = this.plotSurface;

			// List here the plot routines that you want to be accessed
			PlotRoutines = new IDemo [] {    
														new PlotWave(),
													    new PlotDataSet(),
													    new PlotMockup(),
													    new PlotImage(),
													    new PlotQE(),
													    new PlotMarkers(),
														new PlotLogAxis(),
														new PlotLogLog(),
													    new PlotParticles(), 
													    new PlotWavelet(), 
														new PlotSincFunction(), 
														new PlotGaussian(),
														new PlotLabelAxis(),
                									    new PlotCircular(),
                                                        new PlotCandleSimple(),
														new PlotABC(),
												};

			// setup resize handler that takes care of placement of buttons, and sizing of
			// plotsurface2D when window is resized.
			this.Resize += new System.EventHandler(this.ResizeHandler);

			// set up printer
			printDocument = new PrintDocument();
			printDocument.PrintPage += new PrintPageEventHandler(pd_PrintPage);
			int id = currentPlot + 1;
			exampleNumberLabel.Text = "Plot " + id.ToString("0") + "/" + PlotRoutines.Length.ToString("0");

            //this.plotSurface.RightMenu = Florence.WinForms.WinFormsPlotSurface2D.DefaultContextMenu;

            // draw the first plot.
			currentPlot = -1;
            SetPlot(0);
		}


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.quitButton = new System.Windows.Forms.Button();
            this.nextPlotButton = new System.Windows.Forms.Button();
            this.printButton = new System.Windows.Forms.Button();
            this.exampleNumberLabel = new System.Windows.Forms.Label();
            this.prevPlotButton = new System.Windows.Forms.Button();
            this.infoBox = new System.Windows.Forms.TextBox();
            this.plotControl = new Florence.WinForms.PlotControl();
            this.SuspendLayout();
// 
// quitButton
// 
            this.quitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.quitButton.Location = new System.Drawing.Point(248, 386);
            this.quitButton.Name = "quitButton";
            this.quitButton.TabIndex = 14;
            this.quitButton.Text = "Close";
            this.quitButton.Click += new System.EventHandler(this.quitButton_Click);
// 
// nextPlotButton
// 
            this.nextPlotButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nextPlotButton.Location = new System.Drawing.Point(88, 386);
            this.nextPlotButton.Name = "nextPlotButton";
            this.nextPlotButton.TabIndex = 17;
            this.nextPlotButton.Text = "Next";
            this.nextPlotButton.Click += new System.EventHandler(this.nextPlotButton_Click);
// 
// printButton
// 
            this.printButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.printButton.Location = new System.Drawing.Point(166, 386);
            this.printButton.Name = "printButton";
            this.printButton.TabIndex = 9;
            this.printButton.Text = "Print";
            this.printButton.Click += new System.EventHandler(this.printButton_Click);
// 
// exampleNumberLabel
// 
            this.exampleNumberLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.exampleNumberLabel.Location = new System.Drawing.Point(336, 390);
            this.exampleNumberLabel.Name = "exampleNumberLabel";
            this.exampleNumberLabel.Size = new System.Drawing.Size(72, 23);
            this.exampleNumberLabel.TabIndex = 16;
// 
// prevPlotButton
// 
            this.prevPlotButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.prevPlotButton.Location = new System.Drawing.Point(8, 386);
            this.prevPlotButton.Name = "prevPlotButton";
            this.prevPlotButton.TabIndex = 15;
            this.prevPlotButton.Text = "Prev";
            this.prevPlotButton.Click += new System.EventHandler(this.prevPlotButton_Click);
// 
// infoBox
// 
            this.infoBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.infoBox.AutoSize = false;
            this.infoBox.Location = new System.Drawing.Point(13, 416);
            this.infoBox.Multiline = true;
            this.infoBox.Name = "infoBox";
            this.infoBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.infoBox.Size = new System.Drawing.Size(611, 92);
            this.infoBox.TabIndex = 18;
// 
// plotSurface
// 
            this.plotControl.BackColor = System.Drawing.SystemColors.Control;
            this.plotControl.Location = new System.Drawing.Point(8, 8);
            this.plotControl.Name = "plotSurface";
            this.plotControl.Size = new System.Drawing.Size(616, 360);
            this.plotControl.TabIndex = 13;
// 
// PlotSurface2DDemo
// 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(631, 520);
            this.Controls.Add(this.infoBox);
            this.Controls.Add(this.plotControl);
            this.Controls.Add(this.quitButton);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.prevPlotButton);
            this.Controls.Add(this.exampleNumberLabel);
            this.Controls.Add(this.nextPlotButton);
            this.Name = "PlotSurface2DDemo";
            this.Text = "Florence C# Demo";
            this.ResumeLayout(false);

        }
		#endregion


		// The PrintPage event is raised for each page to be printed.
		private void pd_PrintPage(object sender, PrintPageEventArgs ev) 
		{
			// The windows.forms PlotSurface2D control can also be 
			// rendered to other Graphics surfaces. Here we output to a
			// printer. 
			plotSurface.Draw( ev.Graphics, ev.MarginBounds );
			ev.HasMorePages = false;
		}
	

		/// <summary>
		/// callback for quit button click
		/// </summary>
		/// <param name="sender">unused</param>
		/// <param name="e">unused</param>
		private void quitButton_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}


		/// <summary>
		/// callback for resize event.
		/// </summary>
		/// <param name="sender">unused</param>
		/// <param name="e">unused</param>
		private void ResizeHandler(object sender, System.EventArgs e)
		{
			//plotSurface.Width = this.Width - 28;
			//plotSurface.Height = this.Height - 100;

			//nplotLinkLabel.Top = this.Height - 60;
            /*
			nextPlotButton.Top = this.Height - 64;
			prevPlotButton.Top = this.Height - 64;
			printButton.Top = this.Height - 64;
			quitButton.Top = this.Height - 64;
			exampleNumberLabel.Top = this.Height - 60;
            */
		}


		/// <summary>
		/// callback for next button click
		/// </summary>
		/// <param name="sender">unused</param>
		/// <param name="e">unused</param>
		private void nextPlotButton_Click(object sender, System.EventArgs e)
		{
            SetPlot( (currentPlot + 1) % PlotRoutines.Length);
			//this.plotSurface.DateTimeToolTip = false;
		}


		/// <summary>
		/// Callback for Florence link label click
		/// </summary>
		/// <param name="sender">unused</param>
		/// <param name="e">unused</param>
		private void nplotLinkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("www.netcontrols.org/NPlot");
		}


		/// <summary>
		/// callback for print button click
		/// </summary>
		/// <param name="sender">unused</param>
		/// <param name="e">unused</param>
		private void printButton_Click(object sender, System.EventArgs e)
		{
			PrintDialog dlg = new PrintDialog();
			dlg.Document = printDocument;
			if (dlg.ShowDialog() == DialogResult.OK) 
			{
				try
				{
					printDocument.Print();
				}
				catch
				{
					Console.WriteLine( "problem printing.\n" );
				}
			}	
		}


		/// <summary>
		/// Callback for prev button click.
		/// </summary>
		/// <param name="sender">unused</param>
		/// <param name="e">unused</param>
		private void prevPlotButton_Click(object sender, System.EventArgs e)
		{
            SetPlot( (currentPlot - 1) % PlotRoutines.Length);
         }

	}
}
