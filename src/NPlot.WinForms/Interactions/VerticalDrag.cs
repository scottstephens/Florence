using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Florence.WinForms.Interactions
{
    /// <summary>
    /// 
    /// </summary>
    public class VerticalDrag : WinFormsInteraction<Florence.Interactions.VerticalDrag>
    {

        public VerticalDrag(Florence.Interactions.VerticalDrag interaction) : base(interaction) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ctr"></param>
        public override bool DoMouseDown(MouseEventArgs e, Control ctr)
        {
            Florence.PlotSurface2D ps = ((WinForms.WinFormsPlotSurface2D)ctr).Inner;

            if (e.X > ps.PlotAreaBoundingBoxCache.Left && e.X < (ps.PlotAreaBoundingBoxCache.Right) &&
                e.Y > ps.PlotAreaBoundingBoxCache.Top && e.Y < ps.PlotAreaBoundingBoxCache.Bottom)
            {
                dragInitiated_ = true;

                lastPoint_.X = e.X;
                lastPoint_.Y = e.Y;
            }

            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ctr"></param>
        /// <param name="lastKeyEventArgs"></param>
        public override bool DoMouseMove(MouseEventArgs e, Control ctr, KeyEventArgs lastKeyEventArgs)
        {
            Florence.PlotSurface2D ps = ((WinForms.WinFormsPlotSurface2D)ctr).Inner;

            if ((e.Button == MouseButtons.Left) && dragInitiated_)
            {

                int diffY = e.Y - lastPoint_.Y;

                ((WinForms.WinFormsPlotSurface2D)ctr).CacheAxes();

                if (ps.YAxis1 != null)
                {
                    Axis axis = ps.YAxis1;
                    PointF pMin = ps.PhysicalYAxis1Cache.PhysicalMin;
                    PointF pMax = ps.PhysicalYAxis1Cache.PhysicalMax;

                    PointF physicalWorldMin = pMin;
                    PointF physicalWorldMax = pMax;
                    physicalWorldMin.Y -= diffY;
                    physicalWorldMax.Y -= diffY;
                    double newWorldMin = axis.PhysicalToWorld(physicalWorldMin, pMin, pMax, false);
                    double newWorldMax = axis.PhysicalToWorld(physicalWorldMax, pMin, pMax, false);
                    axis.WorldMin = newWorldMin;
                    axis.WorldMax = newWorldMax;
                }

                if (ps.YAxis2 != null)
                {
                    Axis axis = ps.YAxis2;
                    PointF pMin = ps.PhysicalYAxis2Cache.PhysicalMin;
                    PointF pMax = ps.PhysicalYAxis2Cache.PhysicalMax;

                    PointF physicalWorldMin = pMin;
                    PointF physicalWorldMax = pMax;
                    physicalWorldMin.Y -= diffY;
                    physicalWorldMax.Y -= diffY;
                    double newWorldMin = axis.PhysicalToWorld(physicalWorldMin, pMin, pMax, false);
                    double newWorldMax = axis.PhysicalToWorld(physicalWorldMax, pMin, pMax, false);
                    axis.WorldMin = newWorldMin;
                    axis.WorldMax = newWorldMax;
                }

                lastPoint_ = new Point(e.X, e.Y);
                this.FireInteractionOccurred();
                return true;
            }

            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ctr"></param>
        public override bool DoMouseUp(MouseEventArgs e, Control ctr)
        {
            if ((e.Button == MouseButtons.Left) && dragInitiated_)
            {
                lastPoint_ = unset_;
                dragInitiated_ = false;
            }

            return false;
        }

        private bool dragInitiated_ = false;
        private Point lastPoint_ = new Point(-1, -1);
        // this is the condition for an unset point
        private Point unset_ = new Point(-1, -1);
    }
}
