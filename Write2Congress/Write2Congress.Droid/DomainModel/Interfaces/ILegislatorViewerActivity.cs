namespace Write2Congress.Droid.Interfaces
{
    public interface ILegislatorViewerActivity
    {
        SearchTextChangedDelegate LegislatorSearchTextChanged { get; set; }

        void ClearLegislatorSearchTextChangedDelegate();
    }

    //Create new delegate type
    public delegate void SearchTextChangedDelegate(string newValue);
}