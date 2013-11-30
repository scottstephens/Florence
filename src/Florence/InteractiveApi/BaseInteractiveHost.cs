/*
 * Florence - A charting library for .NET
 * 
 * BaseInteractiveHost.cs
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
    public abstract class BaseInteractiveHost<T> : InteractiveHost where T : class, InteractiveFigure
    {
        protected List<T> FiguresTyped { get; set; }

        protected int ActiveFigureIndex { get; set; }
        protected T ActiveFigureTyped { get { return this.ActiveFigureIndex < 0 ? null : this.FiguresTyped[this.ActiveFigureIndex]; } }

        public IEnumerable<InteractiveFigure> Figures { get { return this.FiguresTyped; } }
        public int FigureCount { get { return this.FiguresTyped.Count; } }

        public BaseInteractiveHost()
        {
            this.FiguresTyped = new List<T>();
            this.ActiveFigureIndex = -1;
        }

        #region imperative host methods/properties

        public InteractiveFigure ActiveFigure
        {
            get
            {
                return this.ActiveFigureTyped;
            }
            set
            {
                if (value is T)
                    this.setActiveFigure(value as T);
                else
                    throw new FlorenceException("Can only set active figure of ImperativeHost to figure that uses same GUI toolkit as the host.");
            }
        }

        protected InteractiveFigure ActiveOrNewFigure
        {
            get
            {
                var active_figure = this.ActiveFigure;
                if (active_figure == null)
                    active_figure = this.newFigure();
                return active_figure;
            }
        }

        protected void setActiveFigure(T figure)
        {
            bool found = false;
            int ii = 0;
            foreach (var current_figure in this.FiguresTyped)
            {
                if (figure == current_figure)
                {
                    found = true;
                    break;
                }
                ii++;
            }
            if (found)
            {
                this.ActiveFigureIndex = ii;
            }
            else
            {
                throw new FlorenceException("Figure chosen to be active not in set of active figures kept by ImperativeHost");
            }
        }

        public InteractiveFigure newFigure()
        {
            var new_figure = this.createNewFigure();
            this.FiguresTyped.Add(new_figure);
            new_figure.StateChange += handle_figure_state_changed;
            this.ActiveFigureIndex = this.FiguresTyped.Count - 1;
            return new_figure;
        }
        public InteractiveFigure next()
        {
            if (this.ActiveFigureIndex < 0)
                return this.newFigure();
            else
            {
                this.ActiveFigureIndex += 1;
                this.ActiveFigureIndex %= this.FiguresTyped.Count;
                return this.ActiveFigure;
            }
        }
        public InteractiveFigure previous()
        {
            if (this.ActiveFigureIndex < 0)
                return this.newFigure();
            else
            {
                this.ActiveFigureIndex -= 1;
                this.ActiveFigureIndex %= this.FiguresTyped.Count;
                return this.ActiveFigure;
            }
        }

        private void handle_figure_state_changed(InteractiveFigure figure, FigureState state)
        {
            if (state == FigureState.Closed)
            {
                int index = this.FiguresTyped.FindIndex((x) => figure == x);
                if (index >= 0)
                {
                    this.FiguresTyped.RemoveAt(index);
                    if (this.ActiveFigureIndex >= index)
                        this.ActiveFigureIndex = Math.Max(this.ActiveFigureIndex - 1, this.FiguresTyped.Count - 1);
                }
            }
        }
        #endregion

        #region abstract methods
        
        // Abstract methods that must be implemented by derived class
        protected abstract T createNewFigure();
        public abstract void Start();
        public abstract void Stop();
        
        #endregion

        #region IPlotSurface2D implementation

        public void Add(IDrawable p, int zOrder)
        {
            this.ActiveOrNewFigure.Add(p, zOrder);
        }

        public void Add(IDrawable p, PlotSurface2D.XAxisPosition xp, PlotSurface2D.YAxisPosition yp, int zOrder)
        {
            this.ActiveOrNewFigure.Add(p, xp, yp, zOrder);
        }

        public void Add(IDrawable p)
        {
            this.ActiveOrNewFigure.Add(p);
        }

        public void Add(IDrawable p, PlotSurface2D.XAxisPosition xax, PlotSurface2D.YAxisPosition yax)
        {
            this.ActiveOrNewFigure.Add(p, xax, yax);
        }

        public void Clear()
        {
            if (this.ActiveFigure != null)
                this.ActiveFigure.Clear();
        }

        public Legend Legend
        {
            get
            {
                return this.ActiveOrNewFigure.Legend;
            }
            set
            {
                this.ActiveOrNewFigure.Legend = value;
            }
        }

        public int LegendZOrder
        {
            get
            {
                return this.ActiveOrNewFigure.LegendZOrder;
            }
            set
            {
                this.ActiveOrNewFigure.LegendZOrder = value;
            }
        }

        public int SurfacePadding
        {
            get
            {
                return this.ActiveOrNewFigure.SurfacePadding;
            }
            set
            {
                this.ActiveOrNewFigure.SurfacePadding = value;
            }
        }

        public Color PlotBackColor
        {
            set { this.ActiveOrNewFigure.PlotBackColor = value; }
        }

        public System.Drawing.Bitmap PlotBackImage
        {
            set { this.ActiveOrNewFigure.PlotBackImage = value; }
        }

        public IRectangleBrush PlotBackBrush
        {
            set { this.ActiveOrNewFigure.PlotBackBrush = value; }
        }

        public string Title
        {
            get
            {
                return this.ActiveOrNewFigure.Title;
            }
            set
            {
                this.ActiveOrNewFigure.Title = value;
            }
        }

        public bool AutoScaleTitle
        {
            get
            {
                return this.ActiveOrNewFigure.AutoScaleTitle;
            }
            set
            {
                this.ActiveOrNewFigure.AutoScaleTitle = value;
            }
        }

        public bool AutoScaleAutoGeneratedAxes
        {
            get
            {
                return this.ActiveOrNewFigure.AutoScaleAutoGeneratedAxes;
            }
            set
            {
                this.ActiveOrNewFigure.AutoScaleAutoGeneratedAxes = value;
            }
        }

        public Color TitleColor
        {
            set { this.ActiveOrNewFigure.TitleColor = value; }
        }

        public Brush TitleBrush
        {
            get
            {
                return this.ActiveOrNewFigure.TitleBrush;
            }
            set
            {
                this.ActiveOrNewFigure.TitleBrush = value;
            }
        }

        public Font TitleFont
        {
            get
            {
                return this.ActiveOrNewFigure.TitleFont;
            }
            set
            {
                this.ActiveOrNewFigure.TitleFont = value;
            }
        }

        public System.Drawing.Drawing2D.SmoothingMode SmoothingMode
        {
            get
            {
                return this.ActiveOrNewFigure.SmoothingMode;
            }
            set
            {
                this.ActiveOrNewFigure.SmoothingMode = value;
            }
        }

        public void AddAxesConstraint(AxesConstraint c)
        {
            this.ActiveOrNewFigure.AddAxesConstraint(c);
        }

        public Axis XAxis1
        {
            get
            {
                return this.ActiveOrNewFigure.XAxis1;
            }
            set
            {
                this.ActiveOrNewFigure.XAxis1 = value;
            }
        }

        public Axis XAxis2
        {
            get
            {
                return this.ActiveOrNewFigure.XAxis2;
            }
            set
            {
                this.ActiveOrNewFigure.XAxis2 = value;
            }
        }

        public Axis YAxis1
        {
            get
            {
                return this.ActiveOrNewFigure.YAxis1;
            }
            set
            {
                this.ActiveOrNewFigure.YAxis1 = value;
            }
        }

        public Axis YAxis2
        {
            get
            {
                return this.ActiveOrNewFigure.YAxis2;
            }
            set
            {
                this.ActiveOrNewFigure.YAxis2 = value;
            }
        }

        public void Remove(IDrawable p, bool updateAxes)
        {
            this.ActiveOrNewFigure.Remove(p, updateAxes);
        }

        public System.Collections.ArrayList Drawables
        {
            get { return this.ActiveOrNewFigure.Drawables; }
        }

        #endregion
    }
}
