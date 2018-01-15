using System;
using System.Collections.Generic;
using System.Text;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.DomainModel.Enum;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.DomainModel
{
    public class Legislator : ILegislator
    {
        public Legislator() { }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }

        public Party Party { get; set; }
        public LegislativeBody Chamber { get; set; }
        public StateOrTerritory State { get; set; }
        public Gender Gender { get; set; }

        public DateTime TermStartDate { get; set; }
        public DateTime TermEndDate { get; set; }

        public ContactMethod OfficeAddress { get; set; }
        public ContactMethod OfficeNumber { get; set; }

        public ContactMethod Email { get; set; }
        public ContactMethod FacebookId { get; set; }
        public ContactMethod TwitterId { get; set; }
        public ContactMethod YouTubeId { get; set; }
        public ContactMethod Website { get; set; }
        public ContactMethod ContactSite { get; set; }

        public int TotalVotes { get; set; }
        public float MissedVotesPercent { get; set; }
        public float VotesWithPartyPercent { get; set; }

        public string Senority { get; set; }

        public string IdBioguide { get; set; }
        public string IdGovTrack { get; set; }
        public string IdThomas { get; set; }
        public string IdVoteSmart { get; set; }
        public string IdOpenSecrets { get; set; }

        public string FullName()
        {
            return string.Format("{0} {1}{2}",
                FirstName,
                string.IsNullOrWhiteSpace(MiddleName)
                    ? string.Empty
                    : $"{MiddleName} ",
                LastName);
        }

        public string FormalAddressTitle()
        {
            switch (Chamber)
            {
                case LegislativeBody.Senate:
                    return $"Senator {LastName}";
                case LegislativeBody.House:
                    return string.Format("{0} {1}",
                        Gender == Gender.Female
                            ? "Congresswoman"
                            : "Congressman",
                        LastName);
                case LegislativeBody.Unknown:
                default:
                    return string.Empty;
            }
        }

        public static Legislator TranformToLegislator(ILegislator legislitor)
        {
            var newLegislator = new Legislator()
            {
                IdBioguide = legislitor.IdBioguide,
                Birthday = legislitor.Birthday,
                Chamber = legislitor.Chamber,
                ContactSite = legislitor.ContactSite,
                Email = legislitor.Email,
                FacebookId = legislitor.FacebookId,
                FirstName = legislitor.FirstName,
                Gender = legislitor.Gender,
                LastName = legislitor.LastName,
                MiddleName = legislitor.MiddleName,
                MissedVotesPercent = legislitor.MissedVotesPercent,
                OfficeAddress = legislitor.OfficeAddress,
                OfficeNumber = legislitor.OfficeNumber,
                Party = legislitor.Party,
                Senority = legislitor.Senority,
                State = legislitor.State,
                TermEndDate = legislitor.TermEndDate,
                TermStartDate = legislitor.TermStartDate,
                TotalVotes = legislitor.TotalVotes,
                TwitterId = legislitor.TwitterId,
                VotesWithPartyPercent = legislitor.VotesWithPartyPercent,
                Website = legislitor.Website,
                YouTubeId = legislitor.YouTubeId,

                IdGovTrack  = legislitor.IdGovTrack,
                IdThomas = legislitor.IdThomas,
                IdVoteSmart = legislitor.IdVoteSmart,
                IdOpenSecrets = legislitor.IdOpenSecrets
            };

            return newLegislator;
        }
    }
}
