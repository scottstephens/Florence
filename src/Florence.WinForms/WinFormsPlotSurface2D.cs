/*
 * Florence - A charting library for .NET
 * 
 * WinFormsPlotSurface2d.cs
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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Printing;

using Florence;

namespace Florence.WinForms
{

	/// <summary>
	/// A Windows.Forms PlotSurface2D control.
	/// </summary>
	/// <remarks>
	/// Unfortunately it's not possible to derive from both Control and Florence.PlotSurface2D.
	/// </remarks>
	[ ToolboxBitmapAttribute(typeof(Florence.WinForms.WinFormsPlotSurface2D),"PlotSurface2D.ico") ]
	public partial class WinFormsPlotSurface2D : System.Windows.Forms.UserControl
	{

        public InteractivePlotSurface2D InteractivePlotSurface2D { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public WinFormsPlotSurface2D()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            // double buffer, and update when resize.            
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.ResizeRedraw = true;
		}


        /// <summary>
		/// the paint event callback.
		/// </summary>
		/// <param name="pe">the PaintEventArgs</param>
		protected override void OnPaint( PaintEventArgs pe )
		{
			DoPaint( pe, this.Width, this.Height );
			base.OnPaint(pe);
		}


		/// <summary>
		/// All functionality of the OnPaint method is provided by this function.
		/// This allows use of the all encompasing PlotSurface.
		/// </summary>
		/// <param name="pe">the PaintEventArgs from paint event.</param>
		/// <param name="width">width of the control</param>
		/// <param name="height">height of the control</param>
		public void DoPaint( PaintEventArgs pe, int width, int height )
		{
            Graphics g = pe.Graphics;
			
			Rectangle border = new Rectangle( 0, 0, width, height );

			if ( g == null ) 
			{
				throw (new FlorenceException("null graphics context!"));
			}
			
			if ( this.InteractivePlotSurface2D == null )
			{
				throw (new FlorenceException("null Florence.InteractivePlotSurface2D"));
			}
			
			if ( border == Rectangle.Empty )
			{
				throw (new FlorenceException("null border context"));
			}

			this.Draw( g, border );
		}


		/// <summary>
		/// Draws the plot surface on the supplied graphics surface [not the control surface].
		/// </summary>
		/// <param name="g">The graphics surface on which to draw</param>
		/// <param name="bounds">A bounding box on this surface that denotes the area on the
		/// surface to confine drawing to.</param>
		public void Draw( Graphics g, Rectangle bounds )
		{
			// If we are not in design mode then draw as normal.
			if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) 
			{ 
				this.drawDesignMode( g, bounds );
			}

			this.InteractivePlotSurface2D.Draw( g, bounds );
		}


		/// <summary>
		/// Draw a lightweight representation of us for design mode.
		/// </summary>
		private void drawDesignMode( Graphics g, Rectangle bounds )
		{
			g.DrawRectangle( new Pen(Color.Black), bounds.X + 2, bounds.Y + 2, bounds.Width-4, bounds.Height-4 );			
		}



		/// <summary>
		/// Print the chart as currently shown by the control
		/// </summary>
		/// <param name="preview">If true, show print preview window.</param>
		public void Print( bool preview ) 
		{
			PrintDocument printDocument = new PrintDocument();
			printDocument.PrintPage += new PrintPageEventHandler(Florence_PrintPage);
			printDocument.DefaultPageSettings.Landscape = true;
				 	
			DialogResult result;
            try
            {
                if (!preview)
                {
                    PrintDialog dlg = new PrintDialog();
                    dlg.Document = printDocument;
                    result = dlg.ShowDialog();
                }
                else
                {
                    PrintPreviewDialog dlg = new PrintPreviewDialog();
                    dlg.Document = printDocument;
                    result = dlg.ShowDialog();
                }
                if (result == DialogResult.OK)
                {
                    try
                    {
                        printDocument.Print();
                    }
                    catch
                    {
                        Console.WriteLine("caught\n");
                    }
                }
            }
            catch (InvalidPrinterException)
            {
                Console.WriteLine("caught\n");
            }
		}


		private void Florence_PrintPage(object sender, PrintPageEventArgs ev) 
		{
			Rectangle r = ev.MarginBounds;
			this.Draw( ev.Graphics, r );
			ev.HasMorePages = false;
		}
	
		
		/// <summary>
		/// Coppies the chart currently shown in the control to the clipboard as an image.
		/// </summary>
		public void CopyToClipboard()
		{
			System.Drawing.Bitmap b = new System.Drawing.Bitmap( this.Width, this.Height );
			System.Drawing.Graphics g = Graphics.FromImage( b );
			g.Clear(Color.White);
			this.Draw( g, new Rectangle( 0, 0, b.Width-1, b.Height-1 ) );
			Clipboard.SetDataObject( b, true );
		}


		/// <summary>
		/// Coppies data in the current plot surface view window to the clipboard
		/// as text.
		/// </summary>
		public void CopyDataToClipboard()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			for (int i=0; i<this.InteractivePlotSurface2D.Drawables.Count; ++i)
			{
				IPlot plot = this.InteractivePlotSurface2D.Drawables[i] as IPlot;
				if (plot != null)
				{
					Axis xAxis = this.InteractivePlotSurface2D.WhichXAxis( plot );
					Axis yAxis = this.InteractivePlotSurface2D.WhichYAxis( plot );

					RectangleD region = new RectangleD( 
						xAxis.WorldMin, 
						yAxis.WorldMin,
						xAxis.WorldMax - xAxis.WorldMin,
						yAxis.WorldMax - yAxis.WorldMin );

					plot.WriteData( sb, region, true );
				}
			}

			Clipboard.SetDataObject( sb.ToString(), true );
		}
	}
}
