using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Gtk;

namespace Florence.GtkSharp
{
    public class ImperativeHost : BaseImperativeHost<GtkSharpImperativeFigure>
    {
        public Thread GuiThread { get; private set; }
        public object _main_form;

        public ImperativeHost()
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
        GtkSharpImperativeFigure _tmp_figure;
        protected void createNewFigureInternal(object sender, EventArgs args)
        {
            var tmp_form = new ImperativeFigureForm("");
            var tmp_context = new GtkSharpImperativeFigure(tmp_form);
            tmp_form.ShowAll();
            _tmp_figure = tmp_context;
            _event.Set();
        }

        protected override GtkSharpImperativeFigure createNewFigure()
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
