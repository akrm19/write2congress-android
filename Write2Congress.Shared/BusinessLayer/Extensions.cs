using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.DomainModel.Enum;

namespace Write2Congress.Shared.BusinessLayer
{
    public static class Extensions
    {
        public static List<Legislator> FilterByState(this List<Legislator> legislators, string stateOrTerritorySearchTerm)
        {
            StateOrTerritory stateOrTerritory;

            if (Enum.TryParse<StateOrTerritory>(stateOrTerritorySearchTerm, out stateOrTerritory))
                return legislators.Where(l => l.State == stateOrTerritory).ToList();
            else
                return new List<Legislator>();
        }

        public static List<Legislator> FilterByState(this List<Legislator> legislators, StateOrTerritory stateOrTerritory)
        {
            return stateOrTerritory == StateOrTerritory.ALL
                ? legislators
                : legislators.Where(l => l.State == stateOrTerritory).ToList();
        }

        //public static List<Legislator> FilterByPostalCode(this List<Legislator> legislators, string postalCode)
        //{
        //    return legislators.Where(l => l.)
        //}

        public static List<Legislator> FilterByFirstMiddleOrLastName(this List<Legislator> legislators, string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return legislators.Where(l =>
                l.FirstName.ToLower().Contains(searchTerm)
                //|| l.MiddleName.Contains(searchTerm) TODO: Handle null middle names
                || l.LastName.ToLower().Contains(searchTerm)).ToList();
        }
    }
}
