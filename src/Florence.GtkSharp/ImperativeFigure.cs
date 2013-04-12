using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Florence.GtkSharp
{
    public class GtkSharpImperativeFigure : BaseImperativeFigure<InteractivePlotSurface2D>
    {
        public ImperativeFigureForm HostForm { get; private set; }

        public GtkSharpImperativeFigure(ImperativeFigureForm host_form)
            : base(host_form.PlotSurface)
        {
            this.HostForm = host_form;
            this.HostForm.Destroyed += new EventHandler(HostForm_Destroyed);
        }

        public override event Action<ImperativeFigure, FigureState> StateChange;

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
