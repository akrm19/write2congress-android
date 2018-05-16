using System;
namespace Write2Congress.Droid.DomainModel.Interfaces
{
    public interface IActivityWithToolbarSearch : IDisposable
    {
        FilterDataTextChangedDelegate FilterSearchTextChanged { get; set; }
        FilterDataTextChangedDelegate SearchQuerySubmitted { get; set; }
        ToolbarMenuItemClickedDelegate ExitSearchClicked { get; set; }
        ToolbarMenuItemClickedDelegate FilterSearchviewCollapsed { get; set; }
        ToolbarMenuItemClickedDelegate SearchSearchviewCollapsed { get; set; }

        void ClearFilterTextChangedDelegate();

        void CollapseToolbarSearchview();
        void SetToolbarSearchviewVisibility(bool setAsVisible);
        void SetToolbarExitSearchviewVisibility(bool setAsVisible);
        void SetToolbarFilterviewVisibility(bool setAsVisible);
    }

    //Create new delegate type
    public delegate void FilterDataTextChangedDelegate(string newValue);

    public delegate void ToolbarMenuItemClickedDelegate();
}
