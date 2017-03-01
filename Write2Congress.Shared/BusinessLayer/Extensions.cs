using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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

        public static List<Legislator> FilterByFirstMiddleOrLastName(this List<Legislator> legislators, string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return legislators.Where
                (l =>
                    l.FirstName.ToLower().Contains(searchTerm)
                    || l.LastName.ToLower().Contains(searchTerm)
                    || (!string.IsNullOrWhiteSpace(l.MiddleName) && l.MiddleName.ToLower().Contains(searchTerm))
                ).ToList();
        }

        public static string GetDescription(this Enum enumVal)
        {
            var attr = GetAttributeOfType<DescriptionAttribute>(enumVal);
            return attr != null ? attr.Description : string.Empty;
        }

        public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
        {
            var typeInfo = enumVal.GetType().GetTypeInfo();
            var memberInfo = typeInfo.DeclaredMembers.FirstOrDefault(x => x.Name == enumVal.ToString());

            if (memberInfo == null)
                //throw new InvalidOperationException($"Cannot retrieve attribute. Unable to find memberinfo for {enumVal.ToString()}");
                return null;

            return memberInfo.GetCustomAttribute<T>();
        }
    }
}
