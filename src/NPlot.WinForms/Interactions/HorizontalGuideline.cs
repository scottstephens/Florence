using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Florence.WinForms.Interactions
{
    /// <summary>
    /// Horizontal line interaction
    /// </summary>
    public class HorizontalGuideline : WinFormsInteraction<Florence.Interactions.HorizontalGuideline>
    {
        public HorizontalGuideline(Florence.Interactions.HorizontalGuideline interaction) : base(interaction) { }

        private int barPos_;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pe"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public override void DoPaint(PaintEventArgs pe, int width, int height)
        {
            barPos_ = -1;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ctr"></param>
        /// <param name="lastKeyEventArgs"></param>
        public override bool DoMouseMove(MouseEventArgs e, System.Windows.Forms.Control ctr, KeyEventArgs lastKeyEventArgs)
        {
            Florence.PlotSurface2D ps = ((WinForms.WinFormsPlotSurface2D)ctr).Inner;

            // if mouse isn't in plot region, then don't draw horizontal line
            if (e.X > ps.PlotAreaBoundingBoxCache.Left && e.X < ps.PlotAreaBoundingBoxCache.Right &&
                e.Y > ps.PlotAreaBoundingBoxCache.Top && e.Y < (ps.PlotAreaBoundingBoxCache.Bottom - 1))
            {
                if (ps.PhysicalXAxis1Cache != null)
                {
                    // the clipping rectangle in screen coordinates
                    Rectangle clip = ctr.RectangleToScreen(
                        new Rectangle(
                        (int)ps.PlotAreaBoundingBoxCache.X,
                        (int)ps.PlotAreaBoundingBoxCache.Y,
                        (int)ps.PlotAreaBoundingBoxCache.Width,
                        (int)ps.PlotAreaBoundingBoxCache.Height));

                    Point p = ctr.PointToScreen(new Point(e.X, e.Y));

                    if (barPos_ != -1)
                    {
                        ControlPaint.DrawReversibleLine(
                            new Point(clip.Left, barPos_),
                            new Point(clip.Right, barPos_), this.Interaction.Color);
                    }

                    if (p.Y < clip.Bottom && p.Y > clip.Top)
                    {
                        ControlPaint.DrawReversibleLine(
                            new Point(clip.Left, p.Y),
                            new Point(clip.Right, p.Y), this.Interaction.Color);

                        barPos_ = p.Y;
                    }
                    else
                    {
                        barPos_ = -1;
                    }
                }
            }
            else
            {
                if (barPos_ != -1)
                {
                    Rectangle clip = ctr.RectangleToScreen(
                        new Rectangle(
                        (int)ps.PlotAreaBoundingBoxCache.X,
                        (int)ps.PlotAreaBoundingBoxCache.Y,
                        (int)ps.PlotAreaBoundingBoxCache.Width,
                        (int)ps.PlotAreaBoundingBoxCache.Height));

                    ControlPaint.DrawReversibleLine(
                        new Point(clip.Left, barPos_),
                        new Point(clip.Right, barPos_), this.Interaction.Color);
                    barPos_ = -1;
                }
            }

            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ctr"></param>
        /// <returns></returns>
        public override bool DoMouseLeave(EventArgs e, System.Windows.Forms.Control ctr)
        {
            if (barPos_ != -1)
            {
                Florence.PlotSurface2D ps = ((WinForms.WinFormsPlotSurface2D)ctr).Inner;

                Rectangle clip = ctr.RectangleToScreen(
                    new Rectangle(
                    (int)ps.PlotAreaBoundingBoxCache.X,
                    (int)ps.PlotAreaBoundingBoxCache.Y,
                    (int)ps.PlotAreaBoundingBoxCache.Width,
                    (int)ps.PlotAreaBoundingBoxCache.Height));

                ControlPaint.DrawReversibleLine(
                    new Point(clip.Left, barPos_),
                    new Point(clip.Right, barPos_), this.Interaction.Color);

                barPos_ = -1;
            }
            return false;
        }
    }
}
