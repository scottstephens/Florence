/*
 * Florence - A charting library for .NET
 * 
 * WinFormsInteraction.cs
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
using System.Windows.Forms;
using System.Drawing;

namespace Florence.WinForms
{

    /// <summary>
    /// Base class for an interaction. All methods are virtual. Not abstract as not all interactions
    /// need to use all methods. Default functionality for each method is to do nothing. 
    /// </summary>
    public class WinFormsInteraction<T> : Florence.WinForms.IWinFormsInteraction where T : IInteraction
    {
        public IInteraction GenericInteraction { get { return this.Interaction; } }
        public T Interaction { get; private set; }

        public WinFormsInteraction(T interaction)
        {
            this.Interaction = interaction;
        }

        public event Action<object, IInteraction> InteractionOccurred;

        public event Action<object, T> InteractionOccurredTyped;

        protected void FireInteractionOccurred()
        {
            if (InteractionOccurred != null)
                this.InteractionOccurred(this, this.Interaction);
            if (InteractionOccurredTyped != null)
                this.InteractionOccurredTyped(this, this.Interaction);
        }

        /// <summary>
        /// Handler for this interaction if a mouse down event is received.
        /// </summary>
        /// <param name="e">event args</param>
        /// <param name="ctr">reference to the control</param>
        /// <returns>true if plot surface needs refreshing.</returns>
        public virtual bool DoMouseDown(MouseEventArgs e, System.Windows.Forms.Control ctr) { return false; }

        /// <summary>
        /// Handler for this interaction if a mouse up event is received.
        /// </summary>
        /// <param name="e">event args</param>
        /// <param name="ctr">reference to the control</param>
        /// <returns>true if plot surface needs refreshing.</returns>
        public virtual bool DoMouseUp(MouseEventArgs e, System.Windows.Forms.Control ctr) { return false; }

        /// <summary>
        /// Handler for this interaction if a mouse move event is received.
        /// </summary>
        /// <param name="e">event args</param>
        /// <param name="ctr">reference to the control</param>
        /// <param name="lastKeyEventArgs"></param>
        /// <returns>true if plot surface needs refreshing.</returns>
        public virtual bool DoMouseMove(MouseEventArgs e, System.Windows.Forms.Control ctr, KeyEventArgs lastKeyEventArgs) { return false; }

        /// <summary>
        /// Handler for this interaction if a mouse move event is received.
        /// </summary>
        /// <param name="e">event args</param>
        /// <param name="ctr">reference to the control</param>
        /// <returns>true if plot surface needs refreshing.</returns>
        public virtual bool DoMouseWheel(MouseEventArgs e, System.Windows.Forms.Control ctr) { return false; }

        /// <summary>
        /// Handler for this interaction if a mouse Leave event is received.
        /// </summary>
        /// <param name="e">event args</param>
        /// <param name="ctr">reference to the control</param>
        /// <returns>true if the plot surface needs refreshing.</returns>
        public virtual bool DoMouseLeave(EventArgs e, System.Windows.Forms.Control ctr) { return false; }

        /// <summary>
        /// Handler for this interaction if a paint event is received.
        /// </summary>
        /// <param name="pe">paint event args</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public virtual void DoPaint(PaintEventArgs pe, int width, int height) { }
    }

}
