/*
 * Florence - A charting library for .NET
 * 
 * Interaction.cs
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
using System.Drawing;

namespace Florence
{
    /// <summary>
    /// Encapsulates a number of separate "Interactions". An interaction comprises a number 
    /// of handlers for mouse and keyboard events that work in a specific way, eg rescaling
    /// the axes, scrolling the PlotSurface, etc. 
    /// </summary>
    /// <remarks>
    /// This is a virtual base class, rather than abstract, since particular Interactions will
    /// only require to override a limited number of the possible default handlers. These do
    /// nothing, and return false so that the plot does not require redrawing.
    /// </remarks>
    public class Interaction
    {
        public Interaction()
        {
        }

        /// <summary>
        /// React to the mouse entering the plot
        /// </summary>
        /// <param name="ps">The plot surface to act on.</param>
        /// <returns>True if the plot needs to redraw, false otherwise.</returns>
        public virtual bool DoMouseEnter(InteractivePlotSurface2D ps)
        {
            return false;
        }

        /// <summary>
        /// React to the mouse leaving the plot.
        /// </summary>
        /// <param name="ps">The plot surface to act on.</param>
        /// <returns>True if the plot needs to redraw, false otherwise.</returns>
        public virtual bool DoMouseLeave(InteractivePlotSurface2D ps)
        {
            return false;
        }

        /// <summary>
        /// React to a mouse button being pressed down.
        /// </summary>
        /// <param name="X">X coordinates of the cursor upon mouse down.</param>
        /// <param name="Y">Y coordinates of the curson upon mouse down.</param>
        /// <param name="keys">Indication of which mouse button has been pressed, along with which keyboard keys are also depressed.</param>
        /// <param name="ps">The plot surface to act on.</param>
        /// <returns>True if the plot needs to redraw, false otherwise.</returns>
        public virtual bool DoMouseDown(int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
        {
            return false;
        }

        /// <summary>
        /// React to a mouse button being released.
        /// </summary>
        /// <param name="X">X coordinates of the cursor upon mouse up.</param>
        /// <param name="Y">Y coordinates of the curson upon mouse up.</param>
        /// <param name="keys">Indication of which mouse button has been pressed, along with which keyboard keys are also depressed.</param>
        /// <param name="ps">The plot surface to act on.</param>
        /// <returns>True if the plot needs to redraw, false otherwise.</returns>
        public virtual bool DoMouseUp(int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
        {
            return false;
        }

        /// <summary>
        /// React to the mouse being moved.
        /// </summary>
        /// <param name="X">New X coordinates of the cursor.</param>
        /// <param name="Y">New Y coordinates of the cursor.</param>
        /// <param name="keys">Indication of which mouse buttons and keyboard keys have been pressed.</param>
        /// <param name="ps">The plot surface to act on.</param>
        /// <returns>True if the plot needs to redraw, false otherwise.</returns>
        public virtual bool DoMouseMove(int X, int Y, Modifier keys, InteractivePlotSurface2D ps)
        {
            return false;
        }

        /// <summary>
        /// React to mouse scrolling.
        /// </summary>
        /// <param name="X">X coordinates of the cursor.</param>
        /// <param name="Y">Y coordinates of the cursor.</param>
        /// <param name="direction">Direction of scroll (1 for up, -1 for down).</param>
        /// <param name="keys">Indication of which mouse buttons and keyboard keys have been pressed.</param>
        /// <param name="ps">The plot surface to act on.</param>
        /// <returns>True if the plot needs to redraw, false otherwise.</returns>
        public virtual bool DoMouseScroll(int X, int Y, int direction, Modifier keys, InteractivePlotSurface2D ps)
        {
            return false;
        }

        /// <summary>
        /// React to a key being pressed.
        /// </summary>
        /// <param name="keys">Indication of which keys have been pressed.</param>
        /// <param name="ps">The plot surface to act on.</param>
        /// <returns>True if the plot needs to redraw, false otherwise.</returns>
        public virtual bool DoKeyPress(Modifier keys, InteractivePlotSurface2D ps)
        {
            return false;
        }

        /// <summary>
        /// React to a key being released.
        /// </summary>
        /// <param name="keys">Indication of which keys have been released.</param>
        /// <param name="ps">The plot surface to act on.</param>
        /// <returns>True if the plot needs to redraw, false otherwise.</returns>
        public virtual bool DoKeyRelease(Modifier keys, InteractivePlotSurface2D ps)
        {
            return false;
        }

        /// <summary>
        /// Draw Overlay content over the cached background plot
        /// </summary>
        public virtual void DoDraw(Graphics g, Rectangle dirtyRect)
        {
        }

    }

}
