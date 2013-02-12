using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace NPlot.WinForms.Interactions
{
    /// <summary>
    /// 
    /// </summary>
    public class MouseWheelZoom : WinFormsInteraction<NPlot.Interactions.MouseWheelZoom>
    {
        public MouseWheelZoom(NPlot.Interactions.MouseWheelZoom interaction) : base(interaction) { }

        private Point point_ = new Point(-1, -1);
        //private bool mouseDown_ = false;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ctr"></param>
        public override bool DoMouseUp(MouseEventArgs e, Control ctr)
        {
            //mouseDown_ = false;
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ctr"></param>
        public override bool DoMouseDown(MouseEventArgs e, Control ctr)
        {
            //NPlot.PlotSurface2D ps = ((Windows.PlotSurface2D)ctr).Inner;

            //if (e.X > ps.PlotAreaBoundingBoxCache.Left && e.X < ps.PlotAreaBoundingBoxCache.Right &&
            //    e.Y > ps.PlotAreaBoundingBoxCache.Top && e.Y < ps.PlotAreaBoundingBoxCache.Bottom)
            //{
            //    point_.X = e.X;
            //    point_.Y = e.Y;
            //    mouseDown_ = true;
            //}
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ctr"></param>
        public override bool DoMouseWheel(MouseEventArgs e, Control ctr)
        {
            NPlot.PlotSurface2D ps = ((WinForms.WinFormsPlotSurface2D)ctr).Inner;

            ((WinForms.WinFormsPlotSurface2D)ctr).CacheAxes();

#if API_1_1
                float delta = (float)e.Delta / (float)e.Delta;
#else
            float delta = (float)e.Delta / (float)SystemInformation.MouseWheelScrollDelta;
#endif
            delta *= sensitivity_;
            Axis axis = null;
            PointF pMin = PointF.Empty;
            PointF pMax = PointF.Empty;
            KeyEventArgs keyArgs = ((WinForms.WinFormsPlotSurface2D)ctr).lastKeyEventArgs_;
            bool zoom = (keyArgs != null && keyArgs.Control);

            if (keyArgs != null && keyArgs.Shift)
            {
                axis = ps.YAxis1;
                pMin = ps.PhysicalYAxis1Cache.PhysicalMin;
                pMax = ps.PhysicalYAxis1Cache.PhysicalMax;
            }
            else
            {
                axis = ps.XAxis1;
                pMin = ps.PhysicalXAxis1Cache.PhysicalMin;
                pMax = ps.PhysicalXAxis1Cache.PhysicalMax;
            }
            if (axis == null) return false;

            PointF physicalWorldMin = pMin;
            PointF physicalWorldMax = pMax;
            physicalWorldMin.X -= delta;
            physicalWorldMax.X -= (zoom) ? -delta : delta;
            physicalWorldMin.Y += delta;
            physicalWorldMax.Y += (zoom) ? -delta : delta;
            double newWorldMin = axis.PhysicalToWorld(physicalWorldMin, pMin, pMax, false);
            double newWorldMax = axis.PhysicalToWorld(physicalWorldMax, pMin, pMax, false);
            axis.WorldMin = newWorldMin;
            axis.WorldMax = newWorldMax;

            this.FireInteractionOccurred();

            return true;
        }


        /// <summary>
        /// Number of screen pixels equivalent to one wheel step.
        /// </summary>
        public float Sensitivity
        {
            get
            {
                return sensitivity_;
            }
            set
            {
                sensitivity_ = value;
            }
        }
        private float sensitivity_ = 60.0f;

    }
}
