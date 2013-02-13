/*
 * Florence - A charting library for .NET
 * 
 * PlotContext.cs
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

namespace Florence
{
    public class PlotContext
    {
        protected enum PlotState { Ready, Hidden, Closed }

        public IPlotSurface2D PlotSurface { get; private set; }
        public IInteractivePlotSurface2D InteractivePlotSurface { get; private set; }

        protected PlotState State { get; set; }

        public PlotContext(IPlotSurface2D plot_surface)
        {
            this.PlotSurface = plot_surface;
            this.InteractivePlotSurface = plot_surface as IInteractivePlotSurface2D;            
            this.State = PlotState.Ready;
        }

        public void points(IEnumerable<double> x, IEnumerable<double> y, string x_label="X", string y_label="Y", string title="")
        {
            this.invokeOnGuiThread(() => points_impl(x, y, x_label, y_label, title));
        }

        protected void points_impl(IEnumerable<double> x, IEnumerable<double> y, string x_label = "X", string y_label = "Y", string title = "")
        {
            PointPlot pp = new PointPlot();
            pp.OrdinateData = y;
            pp.AbscissaData = x;
            pp.Marker = new Marker(Marker.MarkerType.FilledCircle, 4, new Pen(Color.Blue));
            this.PlotSurface.Add(pp, Florence.PlotSurface2D.XAxisPosition.Bottom, Florence.PlotSurface2D.YAxisPosition.Left);
            this.PlotSurface.XAxis1.Label = x_label;
            this.PlotSurface.YAxis1.Label = y_label;
            this.PlotSurface.Title = title;
            if (this.InteractivePlotSurface != null)
            {
                this.InteractivePlotSurface.AddInteraction(new Interactions.AxisDrag(false));
                this.InteractivePlotSurface.AddInteraction(new Interactions.HorizontalDrag());
                this.InteractivePlotSurface.AddInteraction(new Interactions.VerticalDrag());
            }
            this.refresh();
        }

        public void clear()
        {
            this.PlotSurface.Clear();
        }

        public virtual void hide()
        {

        }

        public virtual void close()
        {

        }

        public virtual void refresh()
        {

        }

        public virtual void invokeOnGuiThread(Action action)
        {
            action();
        }

    }
}
