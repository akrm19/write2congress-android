using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.BusinessLayer
{
    public class LetterManager
    {
        private ILetterProvider _provider;

        public LetterManager(ILetterProvider letterProvider)
        {
            _provider = letterProvider;
        }

        public List<Letter> GetAllSentLetterByRecipient(Legislator legislator)
        {
            //Todo RM: Verify this will work. Might need to implement equality comparer
            return GettAllSentLetters()
                .Where(l => l.Recipient == legislator)
                .ToList();
        }

        public List<Letter> GettAllSentLetters()
        {
            return GetAllLetters()
                .Where(l => l.Sent == true)
                .OrderBy(lt => lt.DateSent)
                .ToList();
        }

        public List<Letter> GetAllDraftLetters()
        {
            return GetAllLetters()
                .Where(l => l.Sent == false)
                .OrderBy(lt => lt.DateCreated)
                .ToList();
        }

        public bool SaveLetter(Letter letter)
        {
            return _provider.SaveLetter(letter);
        }

        public bool DeleteLetterById(string letterId)
        {
            if(string.IsNullOrWhiteSpace(letterId))
            {
                //TODO add logging
                return true;
            }

            return _provider.DeleteLetterById(letterId);
        }

        #region Helper Methods

        private List<Letter> GetAllLetters()
        {
            return _provider.GetAllLetters() 
                ?? new List<Letter>();
        }

        #endregion
    }
}
