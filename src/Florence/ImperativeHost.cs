using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Florence
{

    public interface ImperativePlottable
    {
        void clear();
        void points(IEnumerable<double> x, IEnumerable<double> y, string x_label="X", string y_label="Y", string title="");
    }

    public interface ImperativeHost : ImperativePlottable
    {
        void Start();
        void Stop();


        IEnumerable<ImperativeFigure> Figures { get; }     
        int FigureCount { get; }
        ImperativeFigure ActiveFigure { get; set; }

        ImperativeFigure newFigure();
        ImperativeFigure next();        
        ImperativeFigure previous();
        
    }

    public abstract class BaseImperativeHost<T> : ImperativeHost where T : class, ImperativeFigure
    {
        protected List<T> FiguresTyped { get; set; }
        
        protected int ActiveFigureIndex { get; set; }
        protected T ActiveFigureTyped { get { return this.ActiveFigureIndex < 0 ? null : this.FiguresTyped[this.ActiveFigureIndex]; } }

        public IEnumerable<ImperativeFigure> Figures { get { return this.FiguresTyped; } }
        public int FigureCount { get { return this.FiguresTyped.Count; } }

        public BaseImperativeHost()
        {
            this.FiguresTyped = new List<T>();
            this.ActiveFigureIndex = -1;
        }

        public ImperativeFigure ActiveFigure
        {
            get
            {
                return this.ActiveFigureTyped;
            }
            set
            {
                if (value is T)
                    this.setActiveFigure(value as T);
                else
                    throw new FlorenceException("Can only set active figure of ImperativeHost to figure that uses same GUI toolkit as the host.");
            }
        }

        protected void setActiveFigure(T figure)
        {
            bool found = false;
            int ii = 0;
            foreach (var current_figure in this.FiguresTyped)
            {
                if (figure == current_figure)
                {
                    found = true;
                    break;
                }
                ii++;
            }
            if (found)
            {
                this.ActiveFigureIndex = ii;
            } 
            else 
            {
                throw new FlorenceException("Figure chosen to be active not in set of active figures kept by ImperativeHost");
            }
        }

        public ImperativeFigure newFigure()
        {
            var new_figure = this.createNewFigure();            
            this.FiguresTyped.Add(new_figure);
            new_figure.StateChange += handle_figure_state_changed;
            this.ActiveFigureIndex = this.FiguresTyped.Count - 1;
            return new_figure;
        }
        public ImperativeFigure next()
        {
            if (this.ActiveFigureIndex < 0)
                return this.newFigure();
            else
            {
                this.ActiveFigureIndex += 1;
                this.ActiveFigureIndex %= this.FiguresTyped.Count;
                return this.ActiveFigure;
            }
        }
        public ImperativeFigure previous()
        {
            if (this.ActiveFigureIndex < 0)
                return this.newFigure();
            else
            {
                this.ActiveFigureIndex -= 1;
                this.ActiveFigureIndex %= this.FiguresTyped.Count;
                return this.ActiveFigure;
            }
        }

        private void handle_figure_state_changed(ImperativeFigure figure, FigureState state)
        {
            if (state == FigureState.Closed)
            {
                int index = this.FiguresTyped.FindIndex((x) => figure == x);
                if (index > 0)
                {
                    this.FiguresTyped.RemoveAt(index);
                    if (this.ActiveFigureIndex >= index)
                        this.ActiveFigureIndex = Math.Max(this.ActiveFigureIndex - 1, this.FiguresTyped.Count - 1);
                }
            }
        }

        // ImperativePlottable methods
        public void clear()
        {
            if (this.ActiveFigure != null)
                this.ActiveFigure.clear();
        }

        public void points(IEnumerable<double> x, IEnumerable<double> y, string x_label = "X", string y_label = "Y", string title = "")
        {
            if (this.ActiveFigureTyped == null)
                this.newFigure();
            this.ActiveFigureTyped.points(x, y, x_label, y_label, title);
        }

        // Abstract methods that must be implemented by derived class
        protected abstract T createNewFigure();
        public abstract void Start();
        public abstract void Stop();
        
    }
}
