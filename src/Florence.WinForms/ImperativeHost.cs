/*
 * Florence - A charting library for .NET
 * 
 * ImperativeHost.cs
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
using System.Windows.Forms;

namespace Florence.WinForms
{

    public class ImperativeHost : BaseImperativeHost<ImperativeFigure>
    {
        public Thread GuiThread { get; private set; }
        protected Control _main_form;

        public ImperativeHost()
        {
            this.GuiThread = null;
        }


        protected void Run()
        {
            _main_form = new Control();

            // This seems to force the control to actually get a handle,
            // which then makes _main_form.InvokeRequired and _main_form.Invoke()
            // use the thread running this method as the GUI thread, which is 
            // the desired behavior in this case.
            System.IntPtr a = _main_form.Handle;

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
            if (_main_form.InvokeRequired)
            {
                _main_form.Invoke(new Action(this.Stop));
            }
            else
            {
                _main_form.Dispose();
            }
            Application.Exit();
            this.GuiThread = null;
        }        

        protected override ImperativeFigure createNewFigure()
        {
            if (_main_form.InvokeRequired)
            {
                return (ImperativeFigure)_main_form.Invoke(new Func<ImperativeFigure>(this.createNewFigure));
            }
            var tmp_form = new ImperativeFigureForm();
            var tmp_context = new ImperativeFigure(tmp_form);            
            tmp_form.Show();
            return tmp_context;
        }

        

    }
}
