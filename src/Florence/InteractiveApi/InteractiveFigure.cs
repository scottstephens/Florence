/*
 * Florence - A charting library for .NET
 * 
 * InteractiveFigure.cs
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
    public enum FigureState { Ready, Hidden, Closed };

    public interface InteractiveFigure : IPlotSurface2D
    {
        /// <summary>
        /// The concrete plot surface that commands issued to this figure will
        /// affect.
        /// </summary>
        IPlotSurface2D PlotSurface { get; }

        /// <summary>
        /// The concrete plot surface that commands issued to this figure will
        /// affect.
        /// </summary>
        InteractivePlotSurface2D InteractivePlotSurface { get; }

        /// <summary>
        /// Hide the figure.
        /// </summary>
        void hide();

        /// <summary>
        /// Make the figure visible.
        /// </summary>
        void show();

        /// <summary>
        /// Permanently close the figure.
        /// </summary>
        void close();

        /// <summary>
        /// Redraw the figure on the screen.
        /// </summary>
        /// <remarks>
        /// Calling this method should not be necessary unless you make changes to the PlotSurface
        /// property directly, which you probably shouldn't do. But if for some reason your plot
        /// seems stale (likely caused by a bug in the library) this might help you solve your 
        /// problem.
        /// </remarks>
        void refresh();

        /// <summary>
        /// Notifies listeners of changes to the state of this figure.
        /// </summary>
        event Action<InteractiveFigure, FigureState> StateChange;
    }
	
}
