/*
 * Florence - A charting library for .NET
 * 
 * InteractiveHost.cs
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
    public interface InteractiveHost : IPlotSurface2D
    {
        /// <summary>
        /// Start the InteractiveHost session. Do this before anything else.
        /// </summary>
        /// <remarks>
        /// The exact details depends on the GUI Toolkit-specific implementation, but
        /// usually this amounts to starting a new thread for the GUI to run on.
        /// </remarks>
        void Start();

        /// <summary>
        /// Stop the InteractiveHost session. Do this when you're all done with plotting.
        /// </summary>
        /// <remarks>
        /// The exact details depend on the GUI Toolkit-specific implementation, but
        /// usually this amounts to closing all open plots and shutting down the GUI thread.
        /// </remarks>
        void Stop();

        /// <summary>
        /// All currently open figures.
        /// </summary>
        IEnumerable<InteractiveFigure> Figures { get; }

        /// <summary>
        /// Number of currently open figures.
        /// </summary>
        int FigureCount { get; }

        /// <summary>
        /// The currently active figure. All plot commands issued directly on this
        /// InteractiveHost object will affect the active figure. If there is no
        /// currently active figure, this will be null. However, most plot commands
        /// will create a new one automatically if there isn't an active figure.
        /// </summary>
        InteractiveFigure ActiveFigure { get; set; }

        /// <summary>
        /// Create a new figure.
        /// </summary>
        /// <returns>The new figure.</returns>
        InteractiveFigure newFigure();

        /// <summary>
        /// Change the active figure to the next one in the list.
        /// </summary>
        /// <returns>The newly active figure.</returns>
        InteractiveFigure next();

        /// <summary>
        /// Change the active figure to the previous one in the list.
        /// </summary>
        /// <returns>The newly active figure.</returns>
        InteractiveFigure previous();

        /// <summary>
        /// Close the currently active figure.
        /// </summary>
        /// <remarks>
        /// After this command is issued, the active figure will become the next figure in the list,
        /// or null if there is only one active figure at the time of the call.
        /// </remarks>
        void closeFigure();
    }
}
