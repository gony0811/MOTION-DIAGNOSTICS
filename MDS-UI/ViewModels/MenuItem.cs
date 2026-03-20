using System;

namespace MDS.UI.ViewModels
{
    public class MenuItem
    {
        public string Label { get; set; }
        public string Icon { get; set; }
        public string ToolTip { get; set; }
        public Uri NavigationDestination { get; set; }
        public Type NavigationType { get; set; }
        public bool IsNavigation => NavigationDestination != null;
    }
}
