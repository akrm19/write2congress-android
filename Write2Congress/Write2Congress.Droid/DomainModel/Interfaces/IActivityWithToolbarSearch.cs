using System;
namespace Write2Congress.Droid.DomainModel.Interfaces
{
    public interface IActivityWithToolbarSearch
    {
        FilterDataTextChangedDelegate LegislatorSearchTextChanged { get; set; }

        void ClearLegislatorSearchTextChangedDelegate();        
    }

    //Create new delegate type
    public delegate void FilterDataTextChangedDelegate(string newValue);
}
