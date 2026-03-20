using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MotionDiagnostics.Navigation
{
    public class FrameAnimator
    {
        public static readonly DependencyProperty FrameNavigationStoryboardProperty
            = DependencyProperty.RegisterAttached(
                "FrameNavigationStoryboard",
                typeof(Storyboard),
                typeof(FrameAnimator),
                new FrameworkPropertyMetadata(null, OnFrameNavigationStoryboardChanged));

        private static void OnFrameNavigationStoryboardChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Frame frame && e.OldValue != e.NewValue)
            {
                frame.Navigating -= Frame_Navigating;
                if (e.NewValue is Storyboard)
                {
                    frame.Navigating += Frame_Navigating;
                }
            }
        }

        private static void Frame_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (sender is Frame frame)
            {
                var sb = GetFrameNavigationStoryboard(frame);
                if (sb != null)
                {
                    var presenter = FindChild<ContentPresenter>(frame);
                    sb.Begin((FrameworkElement)presenter ?? frame);
                }
            }
        }

        private static T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null;

            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T result)
                    return result;

                var found = FindChild<T>(child);
                if (found != null)
                    return found;
            }
            return null;
        }

        /// <summary>Helper for setting <see cref="FrameNavigationStoryboardProperty"/> on <paramref name="control"/>.</summary>
        /// <param name="control"><see cref="DependencyObject"/> to set <see cref="FrameNavigationStoryboardProperty"/> on.</param>
        /// <param name="storyboard">FrameNavigationStoryboard property value.</param>
        public static void SetFrameNavigationStoryboard(DependencyObject control, Storyboard storyboard)
        {
            control.SetValue(FrameNavigationStoryboardProperty, storyboard);
        }

        /// <summary>Helper for getting <see cref="FrameNavigationStoryboardProperty"/> from <paramref name="control"/>.</summary>
        /// <param name="control"><see cref="DependencyObject"/> to read <see cref="FrameNavigationStoryboardProperty"/> from.</param>
        /// <returns>FrameNavigationStoryboard property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        public static Storyboard GetFrameNavigationStoryboard(DependencyObject control)
        {
            return (Storyboard)control.GetValue(FrameNavigationStoryboardProperty);
        }
    }
}
