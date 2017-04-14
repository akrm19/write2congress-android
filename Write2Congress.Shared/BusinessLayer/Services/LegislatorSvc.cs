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
        public List<Legislator> GetAllAlegislators()
        {
            var allLegislatorsUri = "legislators?per_page=all";

            return GetLegislatorsBase(allLegislatorsUri).Result;
        }

        public async Task<List<Legislator>> GetLegislatorsByZipCode(string zipCode)
        {
            var legislatorsByZipUri = "legislators/locate?zip=" + zipCode;

            return GetLegislatorsBase(legislatorsByZipUri).Result;
            /*
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

                    legislators = Util.LegislatorsFromSunlightLegislatorResult(results);
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
            */
        }

        private async Task<List<Legislator>> GetLegislatorsBase(string legislatorsUri)
        {
            var legislators = new List<Legislator>();

            try
            {
                var client = CreateSunlightHttpClient();

                // RestUrl = https://congress.api.sunlightfoundation.com/legislators/locate?zip={0}
                //TODO Ensure this is async
                var response = client.GetAsync(legislatorsUri).Result;

                //var response = await client.GetAsync(legislatorsByZipUri); //TODO Find out why this fails
                //http://stackoverflow.com/questions/10343632/httpclient-getasync-never-returns-when-using-await-async


                if (response.IsSuccessStatusCode)
                {
                    var responseText = response.Content.ReadAsStringAsync().Result;
                    //var responseText = await response.Content.ReadAsStringAsync();
                    var results = JsonConvert.DeserializeObject<SunlightLegislatorResult.Rootobject>(responseText);

                    legislators = Util.LegislatorsFromSunlightLegislatorResult(results);
                }
                else
                {
                    //TODO Add logging and handling
                    //legislators = legislators;
                }
            }
            catch (Exception e)
            {
                var eMessage = e.Message;
            }

            return legislators;
        }
    }
}
