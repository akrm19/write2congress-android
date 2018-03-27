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
        ///  The type for this bill. For the bill “H.R. 4921”, the bill_type represents the 
        /// “H.R.” part. Bill types can be: hr, hres, hjres, hconres, s, sres, sjres, sconres.
        /// </summary>
        /// <param name="bill_type"></param>
        /// <returns></returns>
        public static BillType BillTypeFromText(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return new BillType(BillTypeKind.Empty, string.Empty);

            var billTypeKind = BillTypeKind.Empty;

            switch (type.ToLower())
            {
                case "hr":
                    billTypeKind = BillTypeKind.hr;
                    break;
                case "hres":
                    billTypeKind = BillTypeKind.hres;
                    break;
                case "hjres":
                    billTypeKind = BillTypeKind.hjres;
                    break;
                case "hconres":
                    billTypeKind = BillTypeKind.hconres;
                    break;
                case "s":
                    billTypeKind = BillTypeKind.s;
                    break;
                case "sres":
                    billTypeKind = BillTypeKind.sres;
                    break;
                case "sjres":
                    billTypeKind = BillTypeKind.sjres;
                    break;
                case "sconres":
                    billTypeKind = BillTypeKind.sconres;
                    break;
                default:
                    break;
            }

            return new BillType(billTypeKind, type);
        }

        /*
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
        */

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
        public static LegislativeBody ChamberFromBillProPublica(DomainModel.ApiModels.ProPublica.BillResult.Bill bill)
        {
            if (bill == null || string.IsNullOrWhiteSpace(bill.sponsor_title))
                return LegislativeBody.Unknown;

            if (bill.sponsor_title.StartsWith("rep", StringComparison.OrdinalIgnoreCase))
                return LegislativeBody.House;

            else if (bill.sponsor_title.StartsWith("sen", StringComparison.OrdinalIgnoreCase))
                return LegislativeBody.Senate;

            else
                return LegislativeBody.Unknown;
        }

        public static BillAction LastBillActionFromProPublicaBill(DomainModel.ApiModels.ProPublica.BillResult.Bill bill)
        {
            var lastMajorActionText = bill.latest_major_action;
            var lastMajorActionDate = DateFromSunlightTime(bill.latest_major_action_date);
            var lastMajorActionType = BillActionTypeFromText(lastMajorActionText);

            return new BillAction(lastMajorActionDate, lastMajorActionText, lastMajorActionType);
        }

        public static BillActionType BillActionTypeFromText(string text)
        {
            text = text.ToLower();

            if (text.Contains("became public law"))
                return BillActionType.Enacted;

            if (text.Contains("reffered to"))
            {
                if (text.Contains("subcommittee"))
                    return BillActionType.ReferredToSubcommittee;

                if (text.Contains("committee"))
                    return BillActionType.ReferrdToCommittee;
            }

            if (text.Contains("hearings held"))
                return BillActionType.HearingsHeld;

            return BillActionType.Unknown;
        }

        public static BillStatus BillStatusFromProPublicaBill(DomainModel.ApiModels.ProPublica.BillResult.Bill bill)
        {
            if (!string.IsNullOrWhiteSpace(bill.vetoed))
            {
                var vetoedDate = DateFromSunlightTime(bill.vetoed);

                if (vetoedDate != DateTime.MinValue)
                    return new BillStatus(BillStatusKind.Vetoed, vetoedDate);
            }

            if (!string.IsNullOrWhiteSpace(bill.enacted))
            {
                var enactedDate = DateFromSunlightTime(bill.enacted);
                var billPassedTextIsOk = bill.latest_major_action.StartsWith("Became Public Law", StringComparison.OrdinalIgnoreCase);

                if (enactedDate != DateTime.MinValue)
                {
                    return billPassedTextIsOk
                        ? new BillStatus(BillStatusKind.Enacted, enactedDate, bill.latest_major_action)
                        : new BillStatus(BillStatusKind.Enacted, enactedDate);

                }
            }

            if (DateFromSunlightTime(bill.senate_passage) != DateTime.MinValue && DateFromSunlightTime(bill.house_passage) != DateTime.MinValue)
                return new BillStatus(BillStatusKind.AwaitingSignature);

            if (bill.active)
                return new BillStatus(BillStatusKind.InCongress);

            return new BillStatus(BillStatusKind.Unknown);
        }
    }
}
