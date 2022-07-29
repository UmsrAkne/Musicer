namespace Musicer.Models
{
    using System.Windows.Controls;
    using Microsoft.Xaml.Behaviors;

    public class ScrollBehavior : Behavior<ListView>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SelectionChanged += ScrollBehavior_SelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectionChanged -= ScrollBehavior_SelectionChanged;
        }

        private void ScrollBehavior_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lv = sender as ListView;
            if (lv.SelectedItem != null)
            {
                lv.ScrollIntoView(lv.SelectedItem);
                System.Diagnostics.Debug.WriteLine($"ScrollBehavior : {lv.SelectedItem}");
            }
        }
    }
}
