/*
 * Florence - A charting library for .NET
 * 
 * BaseImperativeFigure.cs
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
    public abstract class BaseImperativeFigure<T> : ImperativeFigure where T : InteractivePlotSurface2D
    {


        public IPlotSurface2D PlotSurface { get { return this.PlotSurfaceTyped; } }
        public InteractivePlotSurface2D InteractivePlotSurface { get { return this.PlotSurfaceTyped; } }

        protected T PlotSurfaceTyped { get; set; }
        protected FigureState State { get; set; }

        public BaseImperativeFigure(T plot_surface)
        {
            this.PlotSurfaceTyped = plot_surface;
            this.StateChange += new Action<ImperativeFigure, FigureState>(BaseImperativeFigure_StateChanged);

        }

        void BaseImperativeFigure_StateChanged(ImperativeFigure arg1, FigureState arg2)
        {
            this.State = arg2;
        }

        private void ensureNotClosed()
        {
            if (this.State == FigureState.Closed)
                throw new FlorenceException("Cannot plot on a closed ImperativeFigure. Create a new one from the ImperativeHost.");
        }

        // ImperativePlottable Implementations
        public void clear()
        {
            this.PlotSurfaceTyped.Clear();
        }

        public void points(IEnumerable<double> x, IEnumerable<double> y, string x_label = "X", string y_label = "Y", string title = "")
        {
            this.ensureNotClosed();
            this.invokeOnGuiThread(() => points_impl(x, y, x_label, y_label, title));
            this.show();
        }

        public void lines(IEnumerable<double> x, IEnumerable<double> y, string x_label = "X", string y_label = "Y", string title = "")
        {
            this.ensureNotClosed();
            this.invokeOnGuiThread(() => lines_impl(x, y, x_label, y_label, title));
            this.show();
        }

        // Actual Plotting Implementations
        protected void points_impl(IEnumerable<double> x, IEnumerable<double> y, string x_label, string y_label, string title)
        {
            PointPlot pp = new PointPlot();
            pp.OrdinateData = y;
            pp.AbscissaData = x;
            pp.Marker = new Marker(Marker.MarkerType.FilledCircle, 4, new Pen(Color.Blue));

            this.do_interactions();

            this.PlotSurface.Add(pp, Florence.PlotSurface2D.XAxisPosition.Bottom, Florence.PlotSurface2D.YAxisPosition.Left);

            this.do_labels(x_label, y_label, title);


            this.refresh();
        }

        protected void lines_impl(IEnumerable<double> x, IEnumerable<double> y, string x_label, string y_label, string title)
        {
            LinePlot pp = new LinePlot();
            pp.OrdinateData = y;
            pp.AbscissaData = x;

            this.do_interactions();

            this.PlotSurface.Add(pp, Florence.PlotSurface2D.XAxisPosition.Bottom, Florence.PlotSurface2D.YAxisPosition.Left);

            this.do_labels(x_label, y_label, title);


            this.refresh();
        }

        private void do_labels(string x_label, string y_label, string title)
        {
            if (x_label == null && this.PlotSurface.XAxis1.Label == "")
                this.PlotSurface.XAxis1.Label = "X";
            else if (x_label != null)
                this.PlotSurface.XAxis1.Label = x_label;

            if (y_label == null && this.PlotSurface.YAxis1.Label == "")
                this.PlotSurface.YAxis1.Label = "Y";
            else if (y_label != null)
                this.PlotSurface.YAxis1.Label = y_label;

            if (title != null)
                this.PlotSurface.Title = title;
        }

        private void do_interactions()
        {
            if (this.PlotSurface.Drawables.Count == 0 && this.InteractivePlotSurface != null)
            {
                this.InteractivePlotSurface.AddInteraction(new AxisDrag());
                this.InteractivePlotSurface.AddInteraction(new PlotDrag(true, true));
            }
        }
        // Abstract methods that must be implemented in a GUI Toolkit specific way
        public abstract void hide();
        public abstract void show();
        public abstract void close();
        public abstract void refresh();
        public abstract void invokeOnGuiThread(Action action);
        public abstract event Action<ImperativeFigure, FigureState> StateChange;
    }


}
