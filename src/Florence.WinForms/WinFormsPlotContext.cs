using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Florence;

namespace Florence.WinForms
{
    public class WinFormsPlotContext : PlotContext
    {
        public InteractivePlotForm HostForm { get; private set; }

        public WinFormsPlotContext(InteractivePlotForm host_form)
            : base(host_form.PlotSurface)
        {
            this.HostForm = host_form;
        }

        public override void hide()
        {
            this.HostForm.Hide();
            this.clear();
        }
                
        public override void close()
        {
            this.HostForm.Close();

        }

        public override void refresh()
        {
            this.HostForm.PlotSurface.Refresh();
        }

        public override void invokeOnGuiThread(Action action)
        {
            if (this.HostForm.InvokeRequired)
                this.HostForm.Invoke(action);
            else
                action();
        }
    }
}
