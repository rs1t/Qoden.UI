﻿using System;
using System.Collections.Generic;
using System.Drawing;
using Qoden.Binding;


#if __IOS__
using UIKit;
using PlatformView = UIKit.UIView;
using PlatformViewGroup = UIKit.UIView;
#endif
#if __ANDROID__
using Android.Views;
using PlatformView = Android.Views.View;
using PlatformViewGroup = Android.Views.ViewGroup;
#endif


namespace Qoden.UI.Wrappers
{
    public struct View
    {
        public static implicit operator PlatformView(View area) { return area.PlatformView; }
        public PlatformView PlatformView { get; set; }

        //You can also use View and other wrapper structs here since all of them 
        //implicitly cnverted to PlatformView subclasses
        public View AddSubview(PlatformView child)
        {
#if __IOS__
            PlatformView.AddSubview(child);
#endif
#if __ANDROID__
            var view = PlatformView as PlatformViewGroup;
            if (view == null) throw new InvalidOperationException("PlatformView is not ViewGroup");
            view.AddView(child);
#endif
            return this;
        }

        public View RemoveFromSuperview()
        {
#if __IOS__
            PlatformView.RemoveFromSuperview();
#endif
#if __ANDROID__
            var view = PlatformView.Parent as PlatformViewGroup;
            if (view == null) throw new InvalidOperationException("PlatformView.Parent is not ViewGroup");
            view.RemoveView(PlatformView);
#endif
            return this;
        }

        public IEnumerable<View> Subviews()
        {
#if __IOS__
            foreach (var v in PlatformView.Subviews)
            {
                yield return new View() { PlatformView = v };
            }
#endif
#if __ANDROID__
            var view = PlatformView as PlatformViewGroup;
            if (view != null)
            {
                var count = view.ChildCount;
                for (int i = 0; i < count; ++i)
                {
                    yield return new View() { PlatformView = view.GetChildAt(i) };
                }
            }
#endif
        }


        public RectangleF Frame
        {
#if __IOS__
            get => (RectangleF)PlatformView.Frame;
            set => PlatformView.Frame = value;
#endif
#if __ANDROID__
            get => new RectangleF(PlatformView.Left, PlatformView.Top, PlatformView.Width, PlatformView.Height);
            set
            {
                PlatformView.Layout((int)Math.Round(value.Left),
                        (int)Math.Round(value.Top),
                        (int)Math.Round(value.Right),
                        (int)Math.Round(value.Bottom));
            }
#endif
        }


        public EdgeInsets Padding
        {
#if __IOS__
            get => new EdgeInsets((float)PlatformView.LayoutMargins.Left,
                                  (float)PlatformView.LayoutMargins.Top,
                                  (float)PlatformView.LayoutMargins.Right,
                                  (float)PlatformView.LayoutMargins.Bottom);
            set 
            {
                PlatformView.LayoutMargins = new UIKit.UIEdgeInsets(value.Left,
                                                  value.Top,
                                                  value.Right,
                                                  value.Bottom);
            }
#endif
#if __ANDROID__
            get => new EdgeInsets(PlatformView.PaddingLeft,
                                  PlatformView.PaddingTop,
                                  PlatformView.PaddingRight,
                                  PlatformView.PaddingBottom);
            set
            {
                PlatformView.SetPadding((int)Math.Round(value.Left),
                                        (int)Math.Round(value.Top),
                                        (int)Math.Round(value.Right),
                                        (int)Math.Round(value.Bottom));
            }
#endif
        }


        public bool Visible
        {
#if __IOS__
            get => !PlatformView.Hidden;
            set => PlatformView.Hidden = !value;
#endif
#if __ANDROID__
            get => PlatformView.Visibility == ViewStates.Visible;
            set => PlatformView.Visibility = value ? ViewStates.Visible : ViewStates.Gone;
#endif
        }

        public View SetBackgroundColor(RGB color)
        {
#if __IOS__
            PlatformView.BackgroundColor = color.ToColor();
#endif
#if __ANDROID__
            PlatformView.SetBackgroundColor(color.ToColor());
#endif
            return this;
        }

        public bool Enabled
        {
#if __IOS__
            get => PlatformView is UIControl ? ((UIControl)PlatformView).Enabled : true;
            set
            {
                if (PlatformView is UIControl)
                { 
                    ((UIControl)PlatformView).Enabled = value;
                }
            }
#endif
#if __ANDROID__
            get => PlatformView.Enabled;
            set => PlatformView.Enabled = value;
#endif
        }

        public SizeF PreferredSize(SizeF size)
        {
            return PlatformView.PreferredSize(size);
        }

        public void SetNeedsLayout()
        {
#if __IOS__
            PlatformView.SetNeedsLayout();
#endif
#if __ANDROID__
            PlatformView.RequestLayout();
#endif
        }

        public EventHandlerSource ClickTarget()
        {
#if __IOS__
            var control = PlatformView as UIControl;
            if (control == null) throw new InvalidOperationException("PlatformView is not UIControl");
            return control.ClickTarget();
#endif
#if __ANDROID__
            return PlatformView.ClickTarget();
#endif
        }

        public override string ToString()
        {
            return PlatformView != null ? PlatformView.ToString() : "Empty QView";
        }
    }

    public static class ViewExtensions
    {
        /// <summary>
        /// Create new empty view and add it to parent
        /// </summary>
        public static View View(this ViewBuilder b)
        {
#if __IOS__
            return new View() { PlatformView = b.AddSubview(new PlatformView()) };
#endif
#if __ANDROID__
            return new View() { PlatformView = b.AddSubview(new PlatformView(b.Context)) };
#endif
        }

        /// <summary>
        /// Wrap convert platform view with cross platform wrapper
        /// </summary>
        public static View AsView(this PlatformView view)
        {
            return new View() { PlatformView = view };
        }
    }
}
