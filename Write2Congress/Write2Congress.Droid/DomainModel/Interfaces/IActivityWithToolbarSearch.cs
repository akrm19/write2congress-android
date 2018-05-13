using System;
namespace Write2Congress.Droid.DomainModel.Interfaces
{
    public interface IActivityWithToolbarSearch
    {
        FilterDataTextChangedDelegate FilterSearchTextChanged { get; set; }

        void ClearFilterTextChangedDelegate();        
    }

    //Create new delegate type
    public delegate void FilterDataTextChangedDelegate(string newValue);
}
