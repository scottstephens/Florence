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
using System.Threading;

using Gtk;

namespace Florence.GtkSharp
{
    public class InteractiveHost : BaseInteractiveHost<InteractiveFigure>
    {
        public Thread GuiThread { get; private set; }
        public object _main_form;

        public InteractiveHost()
        {
            this.GuiThread = null;
        }

        protected void Run()
        {
            //_main_form = new Window("tmp");
            _main_form = new object();
            Application.Init();
            Application.Run();
        }

        public override void Start()
        {
            this.GuiThread = new Thread(Run);
            this.GuiThread.Name = "GuiThread";
            this.GuiThread.Start();
            while (_main_form == null)
                Thread.Sleep(20);
        }

        public override void Stop()
        {
            Application.Quit();
            this.GuiThread = null;
        }

        AutoResetEvent _event;
        object _lock = new object();
        InteractiveFigure _tmp_figure;
        protected void createNewFigureInternal(object sender, EventArgs args)
        {
            var tmp_form = new InteractiveFigureForm("");
            var tmp_context = new InteractiveFigure(tmp_form);
            tmp_form.ShowAll();
            _tmp_figure = tmp_context;
            _event.Set();
        }

        protected override InteractiveFigure createNewFigure()
        {
            lock (_lock)
            {
                _event = new AutoResetEvent(false);
                Gtk.Application.Invoke(createNewFigureInternal);
                _event.WaitOne();
                return _tmp_figure;
            }               
        }

    }
}
