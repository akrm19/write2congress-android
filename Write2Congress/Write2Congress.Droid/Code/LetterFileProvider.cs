using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Write2Congress.Shared.BusinessLayer;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.DomainModel.Interface;
using System.IO;

namespace Write2Congress.Droid.Code
{
    public class LetterFileProvider : ILetterProvider
    {
        private readonly string _lettersExtension = "w2cltrs";
        private string _lettersDirectory;
        private Logger _logger;
        private Util _util;

        public LetterFileProvider()
        {
            _lettersDirectory = Path.Combine(AndroidHelper.GetInternalAppDirPath(), "Letters");
            _logger = new Logger("LetterFileProvider");
            _util = new Util(_logger);

            CreateLetttersDirIfNeeded();
        }

        public List<Letter> GetAllLetters()
        {
            return _util.GetJsonSerializedObjsFromFile<Letter>(_lettersDirectory, _lettersExtension, SearchOption.AllDirectories);
        }

        public bool SaveLetter(Letter letter)
        {
            var serializedLetter = letter.SerializeToJson<Letter>();
            var path = Path.Combine(_lettersDirectory, $"{letter.Id}.{_lettersExtension}");

            return _util.CreateFileContent(path, serializedLetter);
        }

        private void CreateLetttersDirIfNeeded()
        {
            Util.CreateDir(_lettersDirectory);
        }
        
        public bool DeleteLetterById(string letterId)
        {
            var letterPath = Path.Combine(_lettersDirectory, $"{letterId}.{_lettersExtension}");

            return Util.DeleteFile(letterPath, _logger);
        }
    }
}