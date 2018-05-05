using System;
namespace Write2Congress.Droid.DomainModel.Interfaces
{
    public interface IActivityWithToolbarSearch
    {
        SearchTextChangedDelegate LegislatorSearchTextChanged { get; set; }

        void ClearLegislatorSearchTextChangedDelegate();        
    }

    //Create new delegate type
    public delegate void SearchTextChangedDelegate(string newValue);
}
