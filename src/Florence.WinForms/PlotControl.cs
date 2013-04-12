/*
 * Florence - A charting library for .NET
 * 
 * PlotControl.cs
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
	[ ToolboxBitmapAttribute(typeof(Florence.WinForms.PlotControl),"PlotSurface2D.ico") ]
	public partial class PlotControl : System.Windows.Forms.UserControl, IPlotWidget
	{
        InteractivePlotSurface2D Surface;

        public InteractivePlotSurface2D InteractivePlotSurface2D
        {
            get { return this.Surface; }
            set
            {
                this.Surface = value;
                this.InitializePlotSurface();
            }
        }

        private KeyEventArgs lastKeyEventArgs;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public PlotControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            // double buffer, and update when resize.            
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.ResizeRedraw = true;

            this.Surface = null;

            // Map control events to generic events
            this.MouseEnter += new EventHandler(WinFormsPlotSurface2D_MouseEnter);
            this.MouseLeave += new EventHandler(WinFormsPlotSurface2D_MouseLeave);
            this.MouseDown += new MouseEventHandler(WinFormsPlotSurface2D_MouseDown);
            this.MouseMove += new MouseEventHandler(WinFormsPlotSurface2D_MouseMove);
            this.MouseUp += new MouseEventHandler(WinFormsPlotSurface2D_MouseUp);
            this.MouseWheel += new MouseEventHandler(WinFormsPlotSurface2D_MouseWheel);
            this.KeyDown += new KeyEventHandler(WinFormsPlotSurface2D_KeyDown);
            this.KeyUp += new KeyEventHandler(WinFormsPlotSurface2D_KeyUp);
		}

        void InitializePlotSurface()
        {
            this.InteractivePlotSurface2D.DrawQueued += new Action<Rectangle>(InteractivePlotSurface2D_DrawQueued);
            this.InteractivePlotSurface2D.RefreshRequested += new Action(InteractivePlotSurface2D_RefreshRequested);
        }

        void InteractivePlotSurface2D_RefreshRequested()
        {
            this.Invalidate();
        }

        void InteractivePlotSurface2D_DrawQueued(Rectangle obj)
        {
            this.Invalidate(obj);
        }

        void WinFormsPlotSurface2D_KeyUp(object sender, KeyEventArgs e)
        {
            lastKeyEventArgs = e;

            Modifier keys = Key(e.KeyCode);
            keys |= ControlKeys(e);
            this.InteractivePlotSurface2D.DoKeyRelease(keys, this.InteractivePlotSurface2D);
        }

        void WinFormsPlotSurface2D_KeyDown(object sender, KeyEventArgs e)
        {
            lastKeyEventArgs = e;
            Modifier keys = Key(e.KeyCode);
            keys |= ControlKeys(e);
            this.InteractivePlotSurface2D.DoKeyPress(keys, this.InteractivePlotSurface2D);
        }

        void WinFormsPlotSurface2D_MouseWheel(object sender, MouseEventArgs e)
        {
            this.InteractivePlotSurface2D.DoMouseScroll(e.X, e.Y, e.Delta > 0 ? 1 : -1, this.MouseInput(e));
        }

        void WinFormsPlotSurface2D_MouseUp(object sender, MouseEventArgs e)
        {
            this.InteractivePlotSurface2D.DoMouseUp(e.X, e.Y, this.MouseInput(e));
        }

        void WinFormsPlotSurface2D_MouseMove(object sender, MouseEventArgs e)
        {
            this.InteractivePlotSurface2D.DoMouseMove(e.X, e.Y, this.MouseInput(e));
        }
        private Modifier Key(System.Windows.Forms.Keys input)
        {
            switch (input)
            {
                case Keys.Home:
                    return Modifier.Home;
                case Keys.Add:
                    return Modifier.Plus;
                case Keys.Subtract:
                    return Modifier.Minus;
                case Keys.Left:
                    return Modifier.Left;
                case Keys.Right:
                    return Modifier.Right;
                case Keys.Up:
                    return Modifier.Up;
                case Keys.Down:
                    return Modifier.Down;
                default:
                    return Modifier.None;
            }
        }
        private Modifier ControlKeys(KeyEventArgs args)
        {
            Modifier keys = Modifier.None;
            if (args == null)
                return keys;
            if (args.Control)
                keys |= Modifier.Control;
            if (args.Alt)
                keys |= Modifier.Alt;
            if (args.Shift)
                keys |= Modifier.Shift;
            return keys;
        }

        private Modifier MouseInput(MouseEventArgs args)
        {
            Modifier keys = Modifier.None;
            if (args.Button == MouseButtons.Left) keys |= Modifier.Button1;
            if (args.Button == MouseButtons.Middle) keys |= Modifier.Button2;
            if (args.Button == MouseButtons.Right) keys |= Modifier.Button3;
            // Get Control, Alt, etc from last KeyDown
            keys |= ControlKeys(lastKeyEventArgs);
            return keys;
        }

        void WinFormsPlotSurface2D_MouseDown(object sender, MouseEventArgs e)
        {
            this.InteractivePlotSurface2D.DoMouseDown(e.X, e.Y, this.MouseInput(e));
        }

        void WinFormsPlotSurface2D_MouseLeave(object sender, EventArgs e)
        {
            this.InteractivePlotSurface2D.DoMouseLeave(e);
        }

        void WinFormsPlotSurface2D_MouseEnter(object sender, EventArgs e)
        {
            this.InteractivePlotSurface2D.DoMouseEnter(e);
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

			this.InteractivePlotSurface2D.DoDraw( g, bounds );
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
