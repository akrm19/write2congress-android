using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.BusinessLayer;
using Write2Congress.Shared.DomainModel.Enum;
using Write2Congress.Shared.DomainModel.Interface;
using static Write2Congress.Shared.DomainModel.ApiModels.ProPublica.BaseResult;

namespace Write2Congress.Shared.DomainModel.ApiModels.ProPublica
{
    public class VotesResult
    {
        public class Rootobject : BaseRootObject, IVoteResult
        {
            public Result[] results { get; set; }

            List<IVote> IVoteResult.GetVoteResult()
            {
                var votes = new List<IVote>();

                foreach (var votesResult in results.Where(r => r != null && r.votes.Count() > 0))
                    votes.AddRange(votesResult.votes);

                return votes;
            }
        }

        public class Result
        {
            public string member_id { get; set; }
            public string num_results { get; set; }
            public string offset { get; set; }
            public Vote[] votes { get; set; }
        }

        public class Vote : IVote
        {
            public Amendment amendment { get; set; }
            public Bill bill { get; set; }
            public Nomination nomination { get; set; }
            public string chamber { get; set; }
            public string congress { get; set; }
            public string date { get; set; }
            public string description { get; set; }
            public string member_id { get; set; }
            public string position { get; set; }
            public string question { get; set; }
            public string result { get; set; }
            public string roll_call { get; set; }
            public string session { get; set; }
            public string time { get; set; }
            public string vote_uri { get; set; }
            public Total total { get; set; }


			#region IVote Implementation
            string IVote.Description 
            {
                get => string.IsNullOrWhiteSpace(description)
                             ? string.Empty
                             : description;

                set
                {
                    description = value;
                }
            }

            DomainModel.VoteResults IVote.VoteResult
            {
                get
                {
                    if (total == null)
                        return null;

                    var voteResult = new DomainModel.VoteResults();
                    voteResult.No = total.no == null
                        ? -1
                        : (int)total.no;

                    voteResult.NotVoting = total.not_voting == null
                        ? -1
                        : (int)total.not_voting;

                    voteResult.Present = total.present == null
                        ? -1
                        : (int)total.present;

                    voteResult.Yes = total.yes == null
                        ? -1
                        : (int)total.yes;

                    return voteResult;
                }
            }

            VoteCastedType IVote.VoteCastedByLegislator 
            { 
                get
                {
                    return string.IsNullOrWhiteSpace(position)
                        ? VoteCastedType.Unknown
                        : DataTransformationUtil.VoteCastedTypeFromProublicaString(position);
                }
            }

            IBill IVote.Bill 
            { 
                get
                {
                    return bill ?? null;
                }
            }

            string IVote.BillId 
            {
                get
                {
                    return bill?.bill_id;
                }

                set {} 
            }

            LegislativeBody IVote.Chamber 
            { 
                get => DataTransformationUtil.LegislativeBodyFromSunlight(chamber); 
                set {}
            }

            string IVote.NominationId 
            { 
                get
                {
                    return nomination == null || string.IsNullOrWhiteSpace(nomination.nomination_id)
                        ? string.Empty
                        : nomination.nomination_id;  
                }
                set {}
            }

            DomainModel.Nomination IVote.Nomination 
            {
                get
                {
                    if (nomination == null || string.IsNullOrWhiteSpace(nomination.name))
                        return null;

                    return new DomainModel.Nomination(nomination.name, nomination.agency);
                }
                set {}
            }

            string IVote.Question 
            { 
                get => question ?? string.Empty; 
                set => question = value; 
            }

            string IVote.Result 
            {
                get => result ?? string.Empty; 
                set => result = value; 
            }

            string IVote.Source 
            { 
                get => vote_uri ?? string.Empty; 
                set =>vote_uri = value; 
            }

            /*
            VoteType IVote.Type 
            {
                //TODO RM: look if it is used and update/remove if needed
                get => null;//new VoteType(string.Empty, VoteTypeKind.Other); 
                set {} 
            }
            */

            DateTime IVote.VotedAt 
            { 
                get => DataTransformationUtil.DateFromSunlightTime(date); 
                set {}// => throw new NotImplementedException(); 
            }
            #endregion
        }

        public class Bill : IBill
        {
            public string api_uri { get; set; }
            public string bill_id { get; set; }
            public string bill_uri { get; set; }
            public string latest_action { get; set; }
            public string number { get; set; }
            public string sponsor_id { get; set; }
            public string title { get; set; }


            string IBill.BillId 
            {
                get { return bill_id ?? string.Empty; }
                set { bill_id = value; } 
            }
            string IBill.BillNumber 
            { 
                get => number ?? string.Empty; 
                set { number = value; } 
            }

            string IBill.BillUri 
            { 
                get => bill_uri ?? string.Empty; 
                set { bill_uri = value; } 
            }

            BillType IBill.Type => null;

            LegislativeBody IBill.Chamber 
            { 
                get => LegislativeBody.Unknown; 
                set {}
            }

            string IBill.Committees 
            { 
                get => string.Empty; 
                set {}
            }

            int IBill.Congress 
            { 
                get => 0; 
                set {} 
            }

            string IBill.CongressDotGovUrl 
            { 
                get => string.Empty; 
                set {} 
            }

            int IBill.CoSponsorCount => 0;

            DateTime IBill.DateIntroduced 
            { 
                get => DateTime.MinValue;
                set{}
            }

            DateTime IBill.DateLastVoted 
            { 
                get => DateTime.MinValue; 
                set {}
            }

            string IBill.GovTrackUrl 
            { 
                get => string.Empty; 
                set {} 
            }

            BillAction IBill.LastAction 
            { 
                get => new BillAction(DateTime.MinValue, latest_action ?? string.Empty, BillActionType.Unknown); 
                set {}
            }

            int IBill.NumberOfCoSponsors 
            { 
                get => 0; 
                set {} 
            }

            string IBill.SponsorBioId 
            { 
                get => sponsor_id ?? string.Empty; 
                set => sponsor_id = value; 
            }

            string IBill.SponsorName 
            {
                get => string.Empty; 
                set {}
            }

            StateOrTerritory IBill.SponsorState 
            { 
                get => StateOrTerritory.ALL; 
                set {} 
            }

            string IBill.SponsorUri 
            {
                get => string.Empty; 
                set {}
            }

            BillStatus IBill.Status 
            {
                get => new BillStatus(BillStatusKind.Unknown); 
                set {} 
            }

            string IBill.Summary 
            { 
                get => string.Empty; 
                set {} 
            }

            BillTitles IBill.Titles => new BillTitles()
            {
                OfficialTile = title ?? string.Empty
            };
        }

        public class Total
        {
            public int? no { get; set; }
            public int? not_voting { get; set; }
            public int? present { get; set; }
            public int? yes { get; set; }
        }

        public class Nomination
        {
            public string agency { get; set; }
            public string name { get; set; }
            public string nomination_id { get; set; }
            public string number { get; set; }
        }

        public class Amendment
        {
            public string number { get; set; }
            public string api_uri { get; set; }
            public string sponsor_id { get; set; }
            public string sponsor { get; set; }
            public string sponsor_uri { get; set; }
            public string sponsor_party { get; set; }
            public string sponsor_state { get; set; }
        }
    }
}
