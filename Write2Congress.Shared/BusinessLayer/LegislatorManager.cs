using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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

        public byte[] GetLegislatorPortraitAsByteArray(string legislatorBioId)
        {
            var portraitAsByteArray = _legislatorSvc.GetLegislatorPortrait2(legislatorBioId);
            return portraitAsByteArray;
        }

        #region Get Legislator Methods
        public List<ICommittee> GetLegislatorsCommittees(string bioId)
        {
            if (string.IsNullOrWhiteSpace(bioId))
                return null;

            var legislator = _legislatorSvc.GetLegislatorsCommitteesFromProPublica(bioId);

            return legislator;
        }

        public List<Legislator> GetAllLegislators()
        {
            var legislators = new List<Legislator>();
            var iLegislators = _legislatorSvc.GetAllAlegislators();

            foreach (var legislator in iLegislators)
                legislators.Add(Legislator.TranformToLegislator(legislator));

            var legislatorFromUsIoApi = _legislatorSvc.GetLegislatorsFromUsIoApi();

            if(legislatorFromUsIoApi != null && legislatorFromUsIoApi.Count > 0)
                AddMissingDataToLegislatorGroup(legislators, legislatorFromUsIoApi);

            return legislators;
        }

        private void AddMissingDataToLegislatorGroup(List<Legislator> toAddTo, List<ILegislator> source)
        {
            foreach(var legislator in toAddTo)
            {
                var legislatorMatch = source.Where
                    (l => 
                        l.IdBioguide.Equals(legislator.IdBioguide, StringComparison.CurrentCultureIgnoreCase)
                        && legislator.FirstName.Equals(l.FirstName, StringComparison.CurrentCultureIgnoreCase)
                        && legislator.LastName.Equals(l.LastName, StringComparison.CurrentCultureIgnoreCase)
                    ).FirstOrDefault();

                if (legislatorMatch == null) continue;

                AddMissingDataToLegislator(legislator, legislatorMatch);
            }
        }

        private void AddMissingDataToLegislator(Legislator legislator, ILegislator legistlatorWithNewData)
        {
            if (legislator.Gender == Gender.NA && legistlatorWithNewData.Gender != Gender.NA)
                legislator.Gender = legistlatorWithNewData.Gender;

            if (legislator.TermStartDate == DateTime.MinValue && legistlatorWithNewData.TermStartDate != DateTime.MinValue)
                legislator.TermStartDate = legistlatorWithNewData.TermStartDate;

            if (legislator.TermEndDate == DateTime.MinValue && legistlatorWithNewData.TermEndDate != DateTime.MinValue)
                legislator.TermEndDate = legistlatorWithNewData.TermEndDate;

            if (string.IsNullOrWhiteSpace(legislator.IdVoteSmart) && !string.IsNullOrWhiteSpace(legistlatorWithNewData.IdVoteSmart))
                legislator.IdVoteSmart = legistlatorWithNewData.IdVoteSmart;

            if (string.IsNullOrWhiteSpace(legislator.IdOpenSecrets) && !string.IsNullOrWhiteSpace(legistlatorWithNewData.IdOpenSecrets))
                legislator.IdOpenSecrets = legistlatorWithNewData.IdOpenSecrets;
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
