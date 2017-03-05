using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write2Congress.Shared.DomainModel.Interface
{
    public interface ILetterProvider
    {
        bool SaveLetter(Letter letter);
        List<Letter> GetAllLetters();

    }
}
