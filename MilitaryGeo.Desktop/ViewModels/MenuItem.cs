using System.Collections.ObjectModel;

namespace MilitaryGeo.Desktop.ViewModels;

public class MenuItem
{
    public string Icon { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string ViewName { get; set; } = string.Empty;
    public bool HasSubMenu => SubMenuItems != null && SubMenuItems.Count > 0;
    public ObservableCollection<MenuItem>? SubMenuItems { get; set; }
    public bool IsExpanded { get; set; }
}
