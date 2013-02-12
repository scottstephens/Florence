using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace NPlot.WinForms
{

    public class InteractiveHost
    {
        public Thread GuiThread { get; private set; }
        public List<WinFormsPlotContext> Figures { get; private set; }
        protected Control _main_form;
        private WaitHandle _waiter;

        public InteractiveHost()
        {
            this.GuiThread = null;
            this.Figures = new List<WinFormsPlotContext>();
        }

        public void Start()
        {
            this.GuiThread = new Thread(Run);
            this.GuiThread.Name = "GuiThread";
            this.GuiThread.Start();
            while (_main_form == null)
                Thread.Sleep(20);
        }

        public void Stop()
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

        protected void Run()
        {
            _main_form = new Control();
            System.IntPtr a = _main_form.Handle;
            Application.Run();
        }

        public PlotContext newFigure()
        {
            if (_main_form.InvokeRequired)
            {
                return (PlotContext)_main_form.Invoke(new Func<PlotContext>(this.newFigure));
            }
            var tmp_form = new InteractivePlotForm();
            var tmp_context = new WinFormsPlotContext(tmp_form);
            this.Figures.Add(tmp_context);
            tmp_form.Show();
            return tmp_context;
        }

    }
}
