/*
 * Florence - A charting library for .NET
 * 
 * BaseImperativeHost.cs
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

namespace Florence
{
    public abstract class BaseImperativeHost<T> : ImperativeHost where T : class, ImperativeFigure
    {
        protected List<T> FiguresTyped { get; set; }

        protected int ActiveFigureIndex { get; set; }
        protected T ActiveFigureTyped { get { return this.ActiveFigureIndex < 0 ? null : this.FiguresTyped[this.ActiveFigureIndex]; } }

        public IEnumerable<ImperativeFigure> Figures { get { return this.FiguresTyped; } }
        public int FigureCount { get { return this.FiguresTyped.Count; } }

        public BaseImperativeHost()
        {
            this.FiguresTyped = new List<T>();
            this.ActiveFigureIndex = -1;
        }

        public ImperativeFigure ActiveFigure
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

        public ImperativeFigure newFigure()
        {
            var new_figure = this.createNewFigure();
            this.FiguresTyped.Add(new_figure);
            new_figure.StateChange += handle_figure_state_changed;
            this.ActiveFigureIndex = this.FiguresTyped.Count - 1;
            return new_figure;
        }
        public ImperativeFigure next()
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
        public ImperativeFigure previous()
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

        private void handle_figure_state_changed(ImperativeFigure figure, FigureState state)
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

        // ImperativePlottable methods
        public void clear()
        {
            if (this.ActiveFigure != null)
                this.ActiveFigure.clear();
        }

        public void points(IEnumerable<double> x, IEnumerable<double> y, string x_label = "X", string y_label = "Y", string title = "")
        {
            if (this.ActiveFigureTyped == null)
                this.newFigure();
            this.ActiveFigureTyped.points(x, y, x_label, y_label, title);
        }

        public void lines(IEnumerable<double> x, IEnumerable<double> y, string x_label = "X", string y_label = "Y", string title = "")
        {
            if (this.ActiveFigure == null)
                this.newFigure();
            this.ActiveFigureTyped.lines(x, y, x_label, y_label, title);
        }

        // Abstract methods that must be implemented by derived class
        protected abstract T createNewFigure();
        public abstract void Start();
        public abstract void Stop();

    }
}
