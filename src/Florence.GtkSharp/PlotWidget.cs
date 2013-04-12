/*
 * Florence - A charting library for .NET
 * 
 * PlotWidget.cs
 * Copyright (C) 2003-2006 Matt Howlett and others.
 * Copyright (C) 2003-2013 Hywel Thomas
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
using System.Drawing;

using Gtk;
using Florence;

namespace Florence.GtkSharp
{
    public class PlotWidget : DrawingArea, IPlotWidget
    {
        #region boilerplate constructors
        public PlotWidget()
            : base()
        {
            Init();
        }

        public PlotWidget(GLib.GType gtype)
            : base(gtype)
        {
            Init();
        }

        public PlotWidget(IntPtr raw)
            : base(raw)
        {
            Init();
        }
        #endregion

        #region fields
        bool Allocated = false;
        InteractivePlotSurface2D plotSurface;
        #endregion 

        #region properties

        public InteractivePlotSurface2D InteractivePlotSurface2D
        {
            get
            {
                return plotSurface;
            }
            set
            {
                plotSurface = value;
                InitializeSuface();
            }
        }
        #endregion 

        protected void Init()
        {
            this.CanFocus = true;
            this.SizeAllocated += new SizeAllocatedHandler(PlotWidget_SizeAllocated);
            this.ExposeEvent += new ExposeEventHandler(PlotWidget_ExposeEvent);
            this.EnterNotifyEvent += new EnterNotifyEventHandler(PlotWidget_EnterNotifyEvent);
            this.LeaveNotifyEvent += new LeaveNotifyEventHandler(PlotWidget_LeaveNotifyEvent);
            this.ButtonPressEvent += new ButtonPressEventHandler(PlotWidget_ButtonPressEvent);
            this.MotionNotifyEvent += new MotionNotifyEventHandler(PlotWidget_MotionNotifyEvent);
            this.ButtonReleaseEvent += new ButtonReleaseEventHandler(PlotWidget_ButtonReleaseEvent);
            this.ScrollEvent += new ScrollEventHandler(PlotWidget_ScrollEvent);
            this.KeyPressEvent += new KeyPressEventHandler(PlotWidget_KeyPressEvent);
            this.KeyReleaseEvent += new KeyReleaseEventHandler(PlotWidget_KeyReleaseEvent);

            // Subscribe to DrawingArea mouse movement and button press events.
            // Enter and Leave notification is necessary to make ToolTips work.
            // Specify PointerMotionHint to prevent being deluged with motion events.
            this.AddEvents((int)Gdk.EventMask.EnterNotifyMask);
            this.AddEvents((int)Gdk.EventMask.LeaveNotifyMask);
            this.AddEvents((int)Gdk.EventMask.ButtonPressMask);
            this.AddEvents((int)Gdk.EventMask.ButtonReleaseMask);
            this.AddEvents((int)Gdk.EventMask.PointerMotionMask);
            this.AddEvents((int)Gdk.EventMask.PointerMotionHintMask);
            this.AddEvents((int)Gdk.EventMask.ScrollMask);

        }

        void InitializeSuface()
        {
            plotSurface.DrawQueued += new Action<Rectangle>(plotSurface_DrawQueued);
            plotSurface.RefreshRequested += new System.Action(plotSurface_RefreshRequested);
        }

        void plotSurface_DrawQueued(Rectangle obj)
        {
            this.QueueDrawArea(obj.Left, obj.Top, obj.Width, obj.Height);
        }

        void plotSurface_RefreshRequested()
        {
            this.QueueDraw();
        }

        void PlotWidget_KeyReleaseEvent(object o, KeyReleaseEventArgs args)
        {
            Modifier key = Key(args.Event.Key);
            key |= ControlKeys(args.Event.State);
            this.InteractivePlotSurface2D.DoKeyRelease(key, this.InteractivePlotSurface2D);
            args.RetVal = true;
        }

        void PlotWidget_KeyPressEvent(object o, KeyPressEventArgs args)
        {
            Modifier key = Key(args.Event.Key);
            key |= ControlKeys(args.Event.State);
            this.InteractivePlotSurface2D.DoKeyPress(key, this.InteractivePlotSurface2D);
            args.RetVal = true;     // Prevents further key processing
        }

        void PlotWidget_ScrollEvent(object o, ScrollEventArgs args)
        {
            int X, Y;
            int direction = -1;
            Gdk.ModifierType state;

            args.Event.Window.GetPointer(out X, out Y, out state);
            Modifier keys = MouseInput(state);
            if (args.Event.Direction == Gdk.ScrollDirection.Up)
            {
                direction = +1;
            }
            this.InteractivePlotSurface2D.DoMouseScroll(X, Y, direction, keys);
        }

        void PlotWidget_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
        {
            int X, Y;
            Gdk.ModifierType state;

            args.Event.Window.GetPointer(out X, out Y, out state);
            Modifier keys = MouseInput(state);
            this.InteractivePlotSurface2D.DoMouseUp(X, Y, keys);
        }

        void PlotWidget_MotionNotifyEvent(object o, MotionNotifyEventArgs args)
        {
            int X, Y;
            Gdk.ModifierType state;

            // Ensure PlotSurface has keyboard focus
            DrawingArea da = (DrawingArea)o;
            if (!da.HasFocus)
            {
                da.GrabFocus();
            }

            args.Event.Window.GetPointer(out X, out Y, out state);
            Modifier keys = MouseInput(state);
            this.InteractivePlotSurface2D.DoMouseMove(X, Y, keys);
        }
        private Modifier ControlKeys(Gdk.ModifierType state)
        {
            Modifier keys = Modifier.None;
            if ((state & Gdk.ModifierType.ShiftMask) != 0)
                keys |= Modifier.Shift;
            if ((state & Gdk.ModifierType.ControlMask) != 0)
                keys |= Modifier.Control;
            if ((state & Gdk.ModifierType.Mod1Mask) != 0)
                keys |= Modifier.Alt;
            return keys;
        }

        private Modifier Key(Gdk.Key input)
        {
            switch (input)
            {
                case Gdk.Key.Home:
                case Gdk.Key.KP_Home:
                    return Modifier.Home;
                case Gdk.Key.KP_Add:
                case Gdk.Key.plus:
                    return Modifier.Plus;
                case Gdk.Key.KP_Subtract:
                case Gdk.Key.minus:
                    return Modifier.Minus;
                case Gdk.Key.KP_Left:
                case Gdk.Key.Left:
                    return Modifier.Left;
                case Gdk.Key.KP_Right:
                case Gdk.Key.Right:
                    return Modifier.Right;
                case Gdk.Key.KP_Up:
                case Gdk.Key.Up:
                    return Modifier.Up;
                case Gdk.Key.KP_Down:
                case Gdk.Key.Down:
                    return Modifier.Down;
                default:
                    return Modifier.None;
            }
        }

        private Modifier MouseInput(Gdk.ModifierType state)
        {
            Modifier keys = Modifier.None;
            if ((state & Gdk.ModifierType.Button1Mask) != 0) keys |= Modifier.Button1;
            if ((state & Gdk.ModifierType.Button2Mask) != 0) keys |= Modifier.Button2;
            if ((state & Gdk.ModifierType.Button3Mask) != 0) keys |= Modifier.Button3;

            keys |= ControlKeys(state);
            return keys;
        }

        void PlotWidget_ButtonPressEvent(object o, ButtonPressEventArgs args)
        {
            int X, Y;
            Gdk.ModifierType state;

            args.Event.Window.GetPointer(out X, out Y, out state);
            //    state = args.Event.State;
            //    X = (int)args.Event.X;
            //    Y = (int)args.Event.Y;
            Modifier keys = MouseInput(state);
            this.InteractivePlotSurface2D.DoMouseDown(X, Y, keys);
        }

        void PlotWidget_LeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
        {
            this.InteractivePlotSurface2D.DoMouseLeave(args);
        }

        void PlotWidget_EnterNotifyEvent(object o, EnterNotifyEventArgs args)
        {
            if (!this.HasFocus)
                this.GrabFocus();
            this.InteractivePlotSurface2D.DoMouseEnter(args);
        }

        void PlotWidget_ExposeEvent(object o, ExposeEventArgs args)
        {
            Gdk.Window window = args.Event.Window;
            Gdk.Rectangle area = args.Event.Area;   // the Exposed Area

            // This one has trouble with Guideline interaction on single plot, and drag on multiplot
            //Rectangle clip = new Rectangle(area.X, area.Y, area.Width, area.Height);

            // This one makes single plot examples work, but multiplot even more broken
            Rectangle clip = new Rectangle(this.Allocation.Left, this.Allocation.Top, this.Allocation.Width, this.Allocation.Height);

            var window_info = this.WindowPositionAndSize(window);
            
            using (Graphics g = Gtk.DotNet.Graphics.FromDrawable(window, true))
            {
                this.InteractivePlotSurface2D.DoDraw(g, clip);
            }
        }                
        Rectangle WindowPositionAndSize(Gdk.Window window)
        {
            int x,y,width,height;
            window.GetPosition(out x, out y);
            window.GetSize(out width, out height);
            return new Rectangle(x,y,width,height);
        }
        void PlotWidget_SizeAllocated(object o, SizeAllocatedArgs args)
        {
            this.Allocated = true;
            this.QueueDraw();
        }

    }
}
