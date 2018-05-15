using System;
namespace Write2Congress.Droid.DomainModel.Interfaces
{
    public interface IActivityWithToolbarSearch
    {
        FilterDataTextChangedDelegate FilterSearchTextChanged { get; set; }

        FilterDataTextChangedDelegate SearchQuerySubmitted { get; set; }

        void ClearFilterTextChangedDelegate();

        void HideToolbarSearchview();
    }

    //Create new delegate type
    public delegate void FilterDataTextChangedDelegate(string newValue);
}
