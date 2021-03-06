﻿/*
 * Florence - A charting library for .NET
 * 
 * BaseInteractiveFigure.cs
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
    /// <summary>
    /// Base class for implementing GUI Toolkit-specific interactive figures.
    /// </summary>
    /// <typeparam name="T">The type of the GUI Toolkit-specific implementation of InteractivePlotSurface2D</typeparam>
    public abstract class BaseInteractiveFigure<T> : InteractiveFigure where T : InteractivePlotSurface2D
    {

        public IPlotSurface2D PlotSurface { get { return this.PlotSurfaceTyped; } }
        public InteractivePlotSurface2D InteractivePlotSurface { get { return this.PlotSurfaceTyped; } }

        protected T PlotSurfaceTyped { get; set; }
        protected FigureState State { get; set; }

        public BaseInteractiveFigure(T plot_surface)
        {
            this.PlotSurfaceTyped = plot_surface;
            this.StateChange += new Action<InteractiveFigure, FigureState>(BaseImperativeFigure_StateChanged);
        }

        #region imperative figure methods

        void BaseImperativeFigure_StateChanged(InteractiveFigure arg1, FigureState arg2)
        {
            this.State = arg2;
        }

        private void ensureNotClosed()
        {
            if (this.State == FigureState.Closed)
                throw new FlorenceException("Cannot plot on a closed InteractiveFigure. Create a new one from the InteractiveHost.");
        }

        #endregion

        #region abstract methods

        // Abstract methods that must be implemented in a GUI Toolkit specific way
        
        /// <summary>
        /// Hide the figure from view.
        /// </summary>
        public abstract void hide();
        
        /// <summary>
        /// Make the figure visible.
        /// </summary>
        public abstract void show();

        /// <summary>
        /// Permanently close the figure.
        /// </summary>
        public abstract void close();

        /// <summary>
        /// Redraw the control/widget that displays the plot on screen.
        /// </summary>
        public abstract void refresh();

        /// <summary>
        /// Invoke an action on the GUI thread. When clients of this class do something that will modify the plot, it will be done through this method.
        /// </summary>
        /// <param name="action">The action to invoke on the GUI thread.</param>
        public abstract void invokeOnGuiThread(Action action);
        
        /// <summary>
        /// Notifies clients that the state of the figure has changed.
        /// </summary>
        public abstract event Action<InteractiveFigure, FigureState> StateChange;

        #endregion

        #region IPlotSurface2D implementation

        public void Add(IDrawable p, int zOrder)
        {
            this.invokeOnGuiThread(() =>
            {
                this.PlotSurface.Add(p, zOrder);
            });
            this.refresh();
        }

        public void Add(IDrawable p, PlotSurface2D.XAxisPosition xp, PlotSurface2D.YAxisPosition yp, int zOrder)
        {
            this.invokeOnGuiThread(() =>
            {
                this.PlotSurface.Add(p, xp, yp, zOrder);
            });
            this.refresh();
        }

        public void Add(IDrawable p)
        {
            this.invokeOnGuiThread(() =>
            {
                this.PlotSurface.Add(p);
            });
            this.refresh();
        }

        public void Add(IDrawable p, PlotSurface2D.XAxisPosition xax, PlotSurface2D.YAxisPosition yax)
        {
            this.invokeOnGuiThread(() =>
            {
                this.PlotSurface.Add(p, xax, yax);
            });
            this.refresh();
        }

        public void Clear()
        {
            this.invokeOnGuiThread(() =>
            {
                this.PlotSurface.Clear();
            });
            this.refresh();
        }

        public Legend Legend
        {
            get
            {
                return this.PlotSurface.Legend;
            }
            set
            {
                this.invokeOnGuiThread(() =>
                {
                    this.PlotSurface.Legend = value;
                });
                this.refresh();
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
                this.invokeOnGuiThread(() =>
                {
                    this.PlotSurface.LegendZOrder = value;
                });
                this.refresh();
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
                this.invokeOnGuiThread(() =>
                {
                    this.PlotSurface.SurfacePadding = value;
                });
                this.refresh();
            }
        }

        public Color PlotBackColor
        {
            set 
            {
                this.invokeOnGuiThread(() =>
                {
                    this.PlotSurface.PlotBackColor = value;
                });
                this.refresh();
            }
        }

        public System.Drawing.Bitmap PlotBackImage
        {
            set 
            {
                this.invokeOnGuiThread(() =>
                {
                    this.PlotSurface.PlotBackImage = value;
                });
                this.refresh();
            }
        }

        public IRectangleBrush PlotBackBrush
        {
            set 
            {
                this.invokeOnGuiThread(() =>
                {
                    this.PlotSurface.PlotBackBrush = value;
                });
                this.refresh();
            }
        }

        public string Title
        {
            get
            {
                return this.PlotSurface.Title;
            }
            set
            {
                this.invokeOnGuiThread(() =>
                {
                    this.PlotSurface.Title = value;                    
                });
                this.refresh();
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
                this.invokeOnGuiThread(() =>
                {
                    this.PlotSurface.AutoScaleTitle = value;
                });
                this.refresh();
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
                this.invokeOnGuiThread(() =>
                {
                    this.PlotSurface.AutoScaleAutoGeneratedAxes = value;
                });
                this.refresh();
            }
        }

        public Color TitleColor
        {
            set 
            {
                this.invokeOnGuiThread(() =>
                {
                    this.PlotSurface.TitleColor = value;
                });
                this.refresh();
            }
        }

        public Brush TitleBrush
        {
            get
            {
                return this.PlotSurface.TitleBrush;
            }
            set
            {
                this.invokeOnGuiThread(() =>
                {
                    this.PlotSurface.TitleBrush = value;
                });
                this.refresh();
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
                this.invokeOnGuiThread(() =>
                {
                    this.PlotSurface.TitleFont = value;
                });
                this.refresh();
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
                this.invokeOnGuiThread(() =>
                {
                    this.PlotSurface.SmoothingMode = value;
                });
                this.refresh();
            }
        }

        public void AddAxesConstraint(AxesConstraint c)
        {
            this.invokeOnGuiThread(() =>
            {
                this.PlotSurface.AddAxesConstraint(c);
            });
            this.refresh();
        }

        public Axis XAxis1
        {
            get
            {
                return this.PlotSurface.XAxis1;
            }
            set
            {
                this.invokeOnGuiThread(() =>
                {
                    this.PlotSurface.XAxis1 = value;
                });
                this.refresh();
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
                this.invokeOnGuiThread(() =>
                {
                    this.PlotSurface.XAxis2 = value;
                });
                this.refresh();
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
                this.invokeOnGuiThread(() =>
                {
                    this.PlotSurface.YAxis1 = value;
                });
                this.refresh();
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
                this.invokeOnGuiThread(() =>
                {
                    this.PlotSurface.YAxis2 = value;
                });
                this.refresh();
            }
        }

        public void Remove(IDrawable p, bool updateAxes)
        {
            this.invokeOnGuiThread(() =>
            {
                this.PlotSurface.Remove(p, updateAxes);
            });
            this.refresh();
        }

        public System.Collections.ArrayList Drawables
        {
            get { return this.PlotSurface.Drawables; }
        }

        #endregion
    }


}
