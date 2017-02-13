using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.DomainModel.Enum;

namespace Write2Congress.Shared.BusinessLayer.Services
{
    public class LegislatorSvc : ServiceBase
    {

        public async Task<List<Legislator>> GetLegislatorsByZipCode(string zipCode)
        {
            var legislators =  new List<Legislator>();

            try
            {
                var client = CreateSunlightHttpClient();

                // RestUrl = https://congress.api.sunlightfoundation.com/legislators/locate?zip={0}
                var legislatorsByZipUri = "legislators/locate?zip=" + zipCode;

                //TODO Ensure this is async
                var response = client.GetAsync(legislatorsByZipUri).Result;

                //var response = await client.GetAsync(legislatorsByZipUri); //TODO Find out why this fails
                //http://stackoverflow.com/questions/10343632/httpclient-getasync-never-returns-when-using-await-async


                if (response.IsSuccessStatusCode)
                {
                    var responseText = response.Content.ReadAsStringAsync().Result;
                    var results = JsonConvert.DeserializeObject<SunlightLegislatorResult>(responseText);

                    legislators = LegislatorsFromSunlightLegislatorResult(results);
                }
                else
                {
                    //TODO Add logging and handling
                    legislators = legislators;
                }
            }
            catch(Exception e)
            {
                var eMessage = e.Message;
            }

            return legislators;
        }

        private static List<Legislator> LegislatorsFromSunlightLegislatorResult(SunlightLegislatorResult legislatorResults)
        {
            var legislators = new List<Legislator>();


            foreach (var l in legislatorResults.results)
            {
                var legislator = new Legislator()
                {
                    FirstName = l.first_name,
                    MiddleName = l.middle_name,
                    LastName = l.last_name,
                    Birthday = Util.DateFromSunlightTime(l.birthday),
                    Party = Util.PartyFromString(l.party),
                    Chamber = Util.GetLegislativeBodyFromSunlight(l.chamber),
                    State = (State)Enum.Parse(typeof(State), l.state, true),
                    Gender = Util.GenderFromString(l.gender),
                    OfficeAddress = l.office ?? string.Empty,
                    OfficeNumber = l.phone ?? string.Empty,
                    TermStartDate = Util.DateFromSunlightTime(l.term_start),
                    TermEndDate = Util.DateFromSunlightTime(l.term_end),
                    FacebookId = l.facebook_id ?? string.Empty,
                    TwitterId = l.twitter_id ?? string.Empty,
                    YouTubeId = l.youtube_id ?? string.Empty,
                    Website = l.website ?? string.Empty,
                    ContactSite = l.contact_form ?? string.Empty,
                    TotalVotes = 0, //TODO: get rid of the following or populate
                    MissedVotesPercent = 0,
                    VotesWithPartyPercent = 0,
                    Senority = string.Empty
                };

                legislators.Add(legislator);
            }

            return legislators;
        }
    }
}
