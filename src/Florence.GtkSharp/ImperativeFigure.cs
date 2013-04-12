/*
 * Florence - A charting library for .NET
 * 
 * ImperativeFigure.cs
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

namespace Florence.GtkSharp
{
    public class ImperativeFigure : BaseImperativeFigure<InteractivePlotSurface2D>
    {
        public ImperativeFigureForm HostForm { get; private set; }

        public ImperativeFigure(ImperativeFigureForm host_form)
            : base(host_form.PlotSurface)
        {
            this.HostForm = host_form;
            this.HostForm.Destroyed += new EventHandler(HostForm_Destroyed);
        }

        public override event Action<Florence.ImperativeFigure, FigureState> StateChange;

        void HostForm_Destroyed(object sender, EventArgs e)
        {
            if (this.StateChange != null)
                this.StateChange(this, FigureState.Closed);
        }

        public override void hide()
        {
            if (this.State != FigureState.Hidden)
            {
                this.HostForm.Hide();
                if (this.StateChange != null)
                    this.StateChange(this, FigureState.Hidden);
            }
        }

        public override void show()
        {
            if (this.State != FigureState.Ready)
            {
                this.HostForm.Show();
                if (this.StateChange != null)
                    this.StateChange(this, FigureState.Ready);
            }
        }

        public override void close()
        {
            this.HostForm.Destroy();
            if (this.StateChange != null)
                this.StateChange(this, FigureState.Closed);
        }

        public override void refresh()
        {
            this.HostForm.PlotSurface.Refresh();
        }


        private void invokeOnGuiThreadInternal(object sender, EventArgs args)
        {
            Action action = (Action)sender;
            action();
        }

        public override void invokeOnGuiThread(Action action)
        {
            Gtk.Application.Invoke(action, new EventArgs(), invokeOnGuiThreadInternal);            
        }
    }
}
