using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write2Congress.Shared.DomainModel.Interface
{
    public interface ILetterProvider
    {
        List<Letter> GetAllLetters();
        bool SaveLetter(Letter letter);
        bool DeleteLetterById(string letterId);
    }
}
