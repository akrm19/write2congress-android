using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.DomainModel.Enum;

namespace Write2Congress.Shared.BusinessLayer
{
    public static class DataTransformationUtil
    {
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
                case "i":
                case "independent":
                    return Party.Independent;
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
            if (string.IsNullOrWhiteSpace(dateVal))
                return DateTime.MinValue;

            DateTime date;

            if (DateTime.TryParseExact(dateVal, "yyyy-mm-dd", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out date))
                return date;

            return DateTime.TryParseExact(dateVal, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out date)
                ? date
                : DateTime.MinValue;
        }

        public static LegislativeBody LegislativeBodyFromSunlight(string chamber)
        {
            if (string.IsNullOrWhiteSpace(chamber))
                return LegislativeBody.Unknown;

            switch (chamber.ToLower())
            {
                case "sen":
                case "senate":
                    return LegislativeBody.Senate;
                case "rep":
                case "house":
                    return LegislativeBody.House;
                case "joint":
                    return LegislativeBody.Joint;
                default:
                    return LegislativeBody.Unknown;
            }
        }

        public static StateOrTerritory GetStateOrTerritoryFromSunlight(string stateOrTerritory)
        {
            StateOrTerritory result = StateOrTerritory.ALL;

            if (string.IsNullOrWhiteSpace(stateOrTerritory))
                return result;

            if (Enum.TryParse<StateOrTerritory>(stateOrTerritory, true, out result))
                return result;

            return result;
        }

        public static VoteCastedType VoteCasedTypeFromSunlight(dynamic voter_ids, string legislatorBioguideId)
        {
            string legislatorsCastedVote = voter_ids[legislatorBioguideId];

            if (string.IsNullOrWhiteSpace(legislatorsCastedVote))
                return VoteCastedType.Unknown;

            switch (legislatorsCastedVote.ToLower())
            {
                case "nay":
                case "\"nay\"":
                    return VoteCastedType.Nay;
                case "yea":
                case "\"yea\"":
                    return VoteCastedType.Yea;
                case "not voting":
                case "notvoting":
                case "\"not voting\"":
                    return VoteCastedType.NotVoting;
                case "present":
                case "\"present\"":
                    return VoteCastedType.Present;
                default:
                    return VoteCastedType.Unknown;
            }
        }

        public static VoteTypeKind VoteTypeKindFromSunlightVoteType(string voteType)
        {
            if (string.IsNullOrWhiteSpace(voteType))
                return VoteTypeKind.Other;

            switch (voteType.ToLower())
            {
                case "cloture":
                    return VoteTypeKind.Cloture;
                case "impeachmen":
                    return VoteTypeKind.Impeachment;
                case "leadership":
                    return VoteTypeKind.Leadership;
                case "nomination":
                    return VoteTypeKind.Nomination;
                case "other":
                    return VoteTypeKind.Other;
                case "passage":
                    return VoteTypeKind.Passage;
                case "quorum":
                    return VoteTypeKind.Quorum;
                case "recommit":
                    return VoteTypeKind.Recommit;
                case "treaty":
                    return VoteTypeKind.Treaty;
                default:
                    return VoteTypeKind.Other;
            }
        }

        /// <summary>
        /// The type of action. Always present. Can be “action” (generic), 
        /// “vote” (passage vote), “vote-aux” (cloture vote), “vetoed”, 
        /// “topresident”, and “enacted”. There can be other values, but 
        /// these are the only ones we support.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static BillActionType BillActionTypeFromSunlight(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return BillActionType.Unknown;

            switch (type.ToLower())
            {
                case "action":
                    return BillActionType.GenericAction;
                case "vote":
                    return BillActionType.PassageVote;
                case "vote-aux":
                    return BillActionType.ClotureVote;
                case "vetoed":
                    return BillActionType.Vetoed;
                case "topresident":
                    return BillActionType.ToPresident;
                case "enacted":
                    return BillActionType.Enacted;
                default:
                    return BillActionType.Unknown;
            }
        }

        public static LegislativeBillVote LegislativeBillVoteFromSunlight(string passageResult)
        {
            if (string.IsNullOrWhiteSpace(passageResult))
                return LegislativeBillVote.Na;

            switch (passageResult.ToLower())
            {
                case "pass":
                    return LegislativeBillVote.Pass;
                case "fail":
                    return LegislativeBillVote.Fail;
                default:
                    return LegislativeBillVote.Na;
            }
        }
    }
}
