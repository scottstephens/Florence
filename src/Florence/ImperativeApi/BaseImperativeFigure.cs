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

        #region imperative figure methods

        void BaseImperativeFigure_StateChanged(ImperativeFigure arg1, FigureState arg2)
        {
            this.State = arg2;
        }

        private void ensureNotClosed()
        {
            if (this.State == FigureState.Closed)
                throw new FlorenceException("Cannot plot on a closed ImperativeFigure. Create a new one from the ImperativeHost.");
        }

        private void do_interactions()
        {
            if (this.PlotSurface.Drawables.Count == 0 && this.InteractivePlotSurface != null)
            {
                this.InteractivePlotSurface.AddInteraction(new AxisDrag());
                this.InteractivePlotSurface.AddInteraction(new PlotDrag(true, true));
            }
        }

        #endregion

        #region abstract methods

        // Abstract methods that must be implemented in a GUI Toolkit specific way
        public abstract void hide();
        public abstract void show();
        public abstract void close();
        public abstract void refresh();
        public abstract void invokeOnGuiThread(Action action);
        public abstract event Action<ImperativeFigure, FigureState> StateChange;

        #endregion

        #region IPlotSurface2D implementation

        public void Add(IDrawable p, int zOrder)
        {
            this.PlotSurface.Add(p, zOrder);
        }

        public void Add(IDrawable p, PlotSurface2D.XAxisPosition xp, PlotSurface2D.YAxisPosition yp, int zOrder)
        {
            this.PlotSurface.Add(p, xp, yp, zOrder);
        }

        public void Add(IDrawable p)
        {
            this.PlotSurface.Add(p);
        }

        public void Add(IDrawable p, PlotSurface2D.XAxisPosition xax, PlotSurface2D.YAxisPosition yax)
        {
            this.PlotSurface.Add(p, xax, yax);
        }

        public void Clear()
        {
            this.PlotSurface.Clear();
        }

        public Legend Legend
        {
            get
            {
                return this.PlotSurface.Legend;
            }
            set
            {
                this.PlotSurface.Legend = value;
            }
        }

        public int LegendZOrder
        {
            get
            {
                return this.PlotSurface.LegendZOrder;
            }
            set
            {
                this.PlotSurface.LegendZOrder = value;
            }
        }

        public int SurfacePadding
        {
            get
            {
                return this.PlotSurface.SurfacePadding;
            }
            set
            {
                this.PlotSurface.SurfacePadding = value;
            }
        }

        public Color PlotBackColor
        {
            set { this.PlotSurface.PlotBackColor = value; }
        }

        public System.Drawing.Bitmap PlotBackImage
        {
            set { this.PlotSurface.PlotBackImage = value; }
        }

        public IRectangleBrush PlotBackBrush
        {
            set { this.PlotSurface.PlotBackBrush = value; }
        }

        public string Title
        {
            get
            {
                return this.PlotSurface.Title;
            }
            set
            {
                this.PlotSurface.Title = value;
            }
        }

        public bool AutoScaleTitle
        {
            get
            {
                return this.PlotSurface.AutoScaleTitle;
            }
            set
            {
                this.PlotSurface.AutoScaleTitle = value;
            }
        }

        public bool AutoScaleAutoGeneratedAxes
        {
            get
            {
                return this.PlotSurface.AutoScaleAutoGeneratedAxes;
            }
            set
            {
                this.PlotSurface.AutoScaleAutoGeneratedAxes = value;
            }
        }

        public Color TitleColor
        {
            set { this.PlotSurface.TitleColor = value; }
        }

        public Brush TitleBrush
        {
            get
            {
                return this.PlotSurface.TitleBrush;
            }
            set
            {
                this.PlotSurface.TitleBrush = value;
            }
        }

        public Font TitleFont
        {
            get
            {
                return this.PlotSurface.TitleFont;
            }
            set
            {
                this.PlotSurface.TitleFont = value;
            }
        }

        public System.Drawing.Drawing2D.SmoothingMode SmoothingMode
        {
            get
            {
                return this.PlotSurface.SmoothingMode;
            }
            set
            {
                this.PlotSurface.SmoothingMode = value;
            }
        }

        public void AddAxesConstraint(AxesConstraint c)
        {
            this.PlotSurface.AddAxesConstraint(c);
        }

        public Axis XAxis1
        {
            get
            {
                return this.PlotSurface.XAxis1;
            }
            set
            {
                this.PlotSurface.XAxis1 = value;
            }
        }

        public Axis XAxis2
        {
            get
            {
                return this.PlotSurface.XAxis2;
            }
            set
            {
                this.PlotSurface.XAxis2 = value;
            }
        }

        public Axis YAxis1
        {
            get
            {
                return this.PlotSurface.YAxis1;
            }
            set
            {
                this.PlotSurface.YAxis1 = value;
            }
        }

        public Axis YAxis2
        {
            get
            {
                return this.PlotSurface.YAxis2;
            }
            set
            {
                this.PlotSurface.YAxis2 = value;
            }
        }

        public void Remove(IDrawable p, bool updateAxes)
        {
            this.PlotSurface.Remove(p, updateAxes);
        }

        public System.Collections.ArrayList Drawables
        {
            get { return this.PlotSurface.Drawables; }
        }

        #endregion
    }


}
