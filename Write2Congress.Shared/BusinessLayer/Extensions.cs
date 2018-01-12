using Newtonsoft.Json;
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
        public static int GetNumberOfEnums(this Enum enumToCount)// where T : Enum
        {
            var numberOfEnums = Enum.GetNames(enumToCount.GetType()).Length;
            return numberOfEnums;
        }

        public static string Capitalize(this string val)
        {
            if (val.Length > 1)
                return char.ToUpper(val[0]) + val.Substring(1);

            else if (val.Length == 1)
                return char.ToUpper(val[0]).ToString();

            return val;
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

        public static string SerializeToJson<T>(this T objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize);
        }

        public static T DeserializeFromJson<T>(this T objectToSerialize, string jsonSerializedContent)
        {
            if (string.IsNullOrWhiteSpace(jsonSerializedContent))
                throw new JsonSerializationException($"Cannot deserialize {objectToSerialize.GetType().ToString()} from an empty string");

            return JsonConvert.DeserializeObject<T>(jsonSerializedContent);
        }

        #region AppSpecific Extensions

        public static List<Legislator> FilterByState(this List<Legislator> legislators, string stateOrTerritorySearchTerm)
        {
            StateOrTerritory stateOrTerritory;

            if (Enum.TryParse<StateOrTerritory>(stateOrTerritorySearchTerm, out stateOrTerritory))
                return legislators.Where(l => l.State == stateOrTerritory).OrderBy(ln => ln.LastName).ToList();
            else
                return new List<Legislator>();
        }

        public static List<Legislator> FilterByState(this List<Legislator> legislators, StateOrTerritory stateOrTerritory)
        {
            return stateOrTerritory == StateOrTerritory.ALL
                ? legislators
                : legislators.Where(l => l.State == stateOrTerritory).OrderBy(ln => ln.LastName).ToList();
        }

        public static List<Legislator> FilterByFirstMiddleOrLastName(this List<Legislator> legislators, string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return legislators.Where
                (l =>
                    l.FirstName.ToLower().Contains(searchTerm)
                    || l.LastName.ToLower().Contains(searchTerm)
                    || (!string.IsNullOrWhiteSpace(l.MiddleName) && l.MiddleName.ToLower().Contains(searchTerm))
                ).OrderBy(ln => ln.LastName).ToList();
        }

        #endregion
    }
}
