using Android.Content;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Qoden.UI
{
    public class CustomViewToolbar : Toolbar
    {
        public FrameLayout CustomViewWrapper { get; }

        public TextView TitleView { get; }

        public GravityFlags Gravity { get; }

        public CustomViewToolbar(Context context, GravityFlags gravity) : base(context)
        {
            Gravity = gravity;

            CustomViewWrapper = new FrameLayout(context);
            CustomViewWrapper.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);

            TitleView = new TextView(context);
            TitleView.Gravity = GravityFlags.CenterVertical;
            TextViewCompat.SetTextAppearance(TitleView, Android.Resource.Style.TextAppearanceDeviceDefaultWidgetActionBarTitle);

            SetTitleView();

            AddView(CustomViewWrapper, new ActionBar.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.MatchParent,
                (int) Gravity));
        }

        public void SetCustomView(Qoden.UI.Wrappers.View view)
        {
            CustomViewWrapper.RemoveAllViews();
            CustomViewWrapper.AddView(view, new ActionBar.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.MatchParent,
                (int) Gravity));
        }

        public void SetTitleView()
        {
            CustomViewWrapper.RemoveAllViews();
            CustomViewWrapper.AddView(TitleView);
        }
    }
}
