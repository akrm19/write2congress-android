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

        public async Task<List<Legislator>> GetLegislatorsByZipCode(int zipCode)
        {
            var legislators =  new List<Legislator>();
            // RestUrl = https://congress.api.sunlightfoundation.com/legislators/locate?zip={0}
            var legislatorsByZipUri = @"legislators/locate?zip=";
            var requestUri = new Uri(string.Format(legislatorsByZipUri, zipCode));
            var client = CreateSunlightHttpClient();

            var response = await client.GetAsync(requestUri);

            if(response.IsSuccessStatusCode)
            { 
                var responseText = await response.Content.ReadAsStringAsync();
                
                legislators = JsonConvert.
            }

            return legislators;
        }
    }
}
