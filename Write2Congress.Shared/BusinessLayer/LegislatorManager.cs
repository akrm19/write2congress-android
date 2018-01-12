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
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.BusinessLayer
{
    public class LegislatorManager
    {
        LegislatorSvc _legislatorSvc;
        IMyLogger MyLogger;

        public LegislatorManager(IMyLogger logger)
        {
            MyLogger = logger;
            _legislatorSvc = new LegislatorSvc(logger);
        }

        /*
        public Task<byte[]> GetLegislatorPortrait(Legislator legislator)
        {
            if (legislator == null || string.IsNullOrWhiteSpace(legislator.BioguideId))
            {
                MyLogger.Error("Cannot retrieve portrait for empty legislator. Returning NULL");
                return null;
            }

            return _legislatorSvc.GetLegislatorPortrait(legislator);
        }
        */

        #region Get Legislator Methods
        /*
        public List<Legislator> GetLegislatorByZipcode(string zipcode)
        {
            var legislators = new List<Legislator>();

            legislators = _legislatorSvc.GetLegislatorsByZipCode(zipcode).Result;

            return legislators;
        }
        */

        public List<Legislator> GetAllLegislators()
        {
            return _legislatorSvc.GetAllAlegislators();
        }

        public bool SaveLegislatorToFile(string filePath, List<Legislator> legislators)
        {
            try
            {
                var serializedLegislators = JsonConvert.SerializeObject(legislators);
                
                File.WriteAllText(filePath, serializedLegislators);

                return true;
            }
            catch (Exception e)
            {
                MyLogger.Error($"Error occurred saving legislator to file {filePath}", e);
                return false;
            }
        }

        public List<Legislator> GetLegislatorsFromFileSource(string legislatorsFilePath)
        {
            var cachedLegislators = new List<Legislator>();

            if (!File.Exists(legislatorsFilePath))
            {
                MyLogger.Error($"File does not exist. Cannot retrieve legislators from file source: {legislatorsFilePath}");
                return cachedLegislators;
            }

            try
            {
                var fileContents = File.ReadAllText(legislatorsFilePath);
                cachedLegislators = JsonConvert.DeserializeObject<List<Legislator>>(fileContents);
            }
            catch (Exception e)
            {
                MyLogger.Error($"Error occurred retrieving legislators from file source: {legislatorsFilePath}", e);
                throw;
            }

            return cachedLegislators;
        }
        #endregion
    }
}
