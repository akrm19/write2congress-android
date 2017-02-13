using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel.Enum;

namespace Write2Congress.Shared.BusinessLayer
{
    public class Util
    {
        public static LegislativeBody GetLegislativeBodyFromSunlight(string chamber)
        {
            switch (chamber.ToLower())
            {
                case "senate":
                    return LegislativeBody.Senate;
                case "house":
                    return LegislativeBody.House;
                default:
                    return LegislativeBody.Unknown;
            }
        }

        public static Party PartyFromString(string party)
        {
            switch (party.ToLower())
            {
                case "r":
                case "republican":
                    return Party.Republican;
                case "d":
                case "democrat":
                    return Party.Democratic;
                case "l":
                case "libertarian":
                    return Party.Libertarian;
                case "g":
                case "green":
                    return Party.Green;
                default:
                    return Party.Unknown;
            }
        }

        public static Gender GenderFromString(string gender)
        {
            switch (gender.ToLower())
            {
                case "m":
                case "male":
                    return Gender.Male;
                case "f":
                case "female":
                    return Gender.Female;
                default:
                    return Gender.NA;
            }
        }

        public static DateTime DateFromSunlightTime(string dateVal)
        {
            DateTime date;

            return DateTime.TryParseExact(dateVal, "yyyy-mm-dd", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out date)
                ? date
                : DateTime.MinValue;
        }


        public bool ValidZipFormat(string zip)
        {
            if (zip.Length != 5)
                return false; //TODO Add logging

            int zipNumber = 0;

            //TODO RM: handle errors, logging or exception
            return int.TryParse(zip, out zipNumber);            
        }
    }
}
