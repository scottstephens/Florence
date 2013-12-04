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
using System.Threading;

namespace Florence.GtkSharp
{
    public class InteractiveFigure : BaseInteractiveFigure<InteractivePlotSurface2D>
    {
        public InteractiveFigureForm HostForm { get; private set; }

        private int InvokeCounter = 0;
        private object lockable = new object();

        public InteractiveFigure(InteractiveFigureForm host_form)
            : base(host_form.PlotSurface)
        {
            this.HostForm = host_form;
            this.HostForm.Destroyed += new EventHandler(HostForm_Destroyed);
        }

        public override event Action<Florence.InteractiveFigure, FigureState> StateChange;

        void HostForm_Destroyed(object sender, EventArgs e)
        {
            if (this.StateChange != null)
                this.StateChange(this, FigureState.Closed);
        }

        public override void hide()
        {
            if (this.State != FigureState.Hidden)
            {
                this.invokeOnGuiThread(() =>
                {
                    this.HostForm.Hide();
                });
                if (this.StateChange != null)
                    this.StateChange(this, FigureState.Hidden);
            }
        }

        public override void show()
        {
            if (this.State != FigureState.Ready)
            {
                this.invokeOnGuiThread(() =>
                {
                    // Do not know how to prevent window from grabbing focus here
                    // this.HostForm.CanFocus = false; before, then = true after doesn't work
                    // this.HostForm.AcceptFocus doesn't seem to do much either
                    this.HostForm.Show();
                });
                if (this.StateChange != null)
                    this.StateChange(this, FigureState.Ready);
            }
            else if (this.State == FigureState.Ready)
            {
                this.invokeOnGuiThread(() =>
                {
                    this.HostForm.KeepAbove = true;
                    this.HostForm.KeepAbove = false;
                });
            }
        }

        public override void close()
        {
            this.invokeOnGuiThread(() =>
            {
                this.HostForm.Destroy();
            });
            if (this.StateChange != null)
                this.StateChange(this, FigureState.Closed);
        }

        public override void refresh()
        {
            this.invokeOnGuiThread(() => this.HostForm.PlotSurface.Refresh());
        }


        private void invokeOnGuiThreadInternal(object sender, EventArgs args)
        {
            var targs = (InvokeOnGuiThreadArgs)args;
            Action action = (Action)sender;
            action();
            targs.Done();
        }

        public override void invokeOnGuiThread(Action action)
        {
            InvokeOnGuiThreadArgs args;
            lock(lockable)
                args = new InvokeOnGuiThreadArgs(this.InvokeCounter++);
            Gtk.Application.Invoke(action, args, invokeOnGuiThreadInternal);
            args.Wait();
        }
    }

    public class InvokeOnGuiThreadArgs : EventArgs
    {
        private ManualResetEvent ResetEvent;
        private int Id;
        public InvokeOnGuiThreadArgs(int id)
        {
            this.Id = id;
            this.ResetEvent = new ManualResetEvent(false);
        }

        public void Done()
        {
            this.ResetEvent.Set();
        }

        public void Wait()
        {
            this.ResetEvent.WaitOne();
        }
    }
}
