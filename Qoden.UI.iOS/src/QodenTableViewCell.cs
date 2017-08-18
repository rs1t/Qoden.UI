﻿using System;
using UIKit;
using Foundation;
using Qoden.Binding;
using System.Drawing;
using CoreGraphics;

namespace Qoden.UI
{
    public class QodenTableViewCell : UITableViewCell, ILayoutable
    {
        public ViewBuilder Builder { get; private set; }

        public QodenTableViewCell(UITableViewCellStyle style, string reuseIdentifier) : base(style, reuseIdentifier)
        {
            Initialize();
        }

        public QodenTableViewCell()
        {
            Initialize();
        }

        public QodenTableViewCell(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public QodenTableViewCell(NSObjectFlag t) : base(t)
        {
            Initialize();
        }

        public QodenTableViewCell(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        public QodenTableViewCell(CoreGraphics.CGRect frame) : base(frame)
        {
            Initialize();
        }

        public QodenTableViewCell(UITableViewCellStyle style, NSString reuseIdentifier) : base(style, reuseIdentifier)
        {
            Initialize();
        }

        void Initialize()
        {
            Builder = new ViewBuilder(ContentView);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            var layout = new LayoutBuilder((RectangleF)Bounds);
            OnLayout(layout);
            foreach (var v in layout.Views)
            {
                v.Layout();
            }
        }

        void ILayoutable.OnLayout(LayoutBuilder layout)
        {
            OnLayout(layout);
        }

        protected virtual void OnLayout(LayoutBuilder layout)
        {
        }

        public sealed override CGSize SizeThatFits(CGSize size)
        {
            return PreferredSize((SizeF)size);
        }

        public virtual SizeF PreferredSize(SizeF bounds)
        {
            return ViewLayoutUtil.PreferredSize(this, bounds);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Builder.Dispose();
                bindings.Unbind();
            }
            base.Dispose(disposing);
        }

        public override void WillMoveToSuperview(UIView newsuper)
        {
            base.WillMoveToSuperview(newsuper);

            if (bindings.HasValue)
            {
                if (newsuper != null)
                {
                    Bindings.Bind();
                    Bindings.UpdateTarget();
                }
                else
                {
                    Bindings.Unbind();
                }
            }
        }

        BindingListHolder bindings;
        public BindingList Bindings => bindings.Value;
    }
}
