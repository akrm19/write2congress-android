using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.BusinessLayer.Services;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.DomainModel.Enum;

namespace Write2Congress.Shared.BusinessLayer
{
    public class LegislatorManager
    {
        LegislatorSvc _legislatorSvc;

        public LegislatorManager()
        {
            _legislatorSvc = new LegislatorSvc();
        }

        #region Get Legislator Methods
        public List<Legislator> GetLegislatorByZipcode(string zipcode)
        {
            var legislators = new List<Legislator>();

            legislators = _legislatorSvc.GetLegislatorsByZipCode(zipcode).Result;

            return legislators;
        }

        public List<Legislator> GetAllLegislators()
        {
            return _legislatorSvc.GetAllAlegislators();
        }

        public static bool SaveLegislatorToFile(string filePath, List<Legislator> legislators)
        {
            try
            {
                var serializedLegislators = JsonConvert.SerializeObject(legislators);
                
                File.WriteAllText(filePath, serializedLegislators);

                return true;
            }
            catch (Exception e)
            {
                //TODO RM: Add logging
                return false;
            }
        }

        public static List<Legislator> GetLegislatorsFromFileSource(string legislatorsFilePath)
        {
            var cachedLegislators = new List<Legislator>();

            if (!File.Exists(legislatorsFilePath))
            {
                //TODO RM: Add logging
                return cachedLegislators;
            }

            try
            {
                var fileContents = File.ReadAllText(legislatorsFilePath);
                cachedLegislators = JsonConvert.DeserializeObject<List<Legislator>>(fileContents);
            }
            catch (Exception e)
            {
                //TODO RM: add logging
                throw;
            }

            return cachedLegislators;
        }
        #endregion
    }
}
