using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
//using Android.Graphics;
using Android.Locations;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.BusinessLayer;
using System.IO;
using Newtonsoft.Json;

namespace Write2Congress.Droid.Code
{
    public class AndroidHelper
    {
        private static Logger _logger = new Logger("AndroidHelper");
        private Util _util = new Util(_logger);

        public AndroidHelper(Logger logger)
        {
            _logger = logger;
            _util = new Util(_logger);
        }


        #region General Android Helpers

        public static Android.OS.BuildVersionCodes GetCurrentSdkVer()
        {
            return Android.OS.Build.VERSION.SdkInt;
        }

        public static void AddFragment(FragmentManager fragmentManager, Fragment fragment, int containerId, string tag)
        {
            var transaction = fragmentManager.BeginTransaction();

            transaction.Add(containerId, fragment, tag);
            transaction.Commit();
        }

        public static void AddSupportFragment(Android.Support.V4.App.FragmentManager fragmentManager, Android.Support.V4.App.Fragment fragment, int containerId, string tag)
        {
            var transaction = fragmentManager.BeginTransaction();
            transaction.Add(containerId, fragment, tag);
            transaction.Commit();
        }

        public static Intent GetSendEmailIntent(string to, string subject, string body, string cc)
        {
            var intent = new Intent(Intent.ActionSend);

            if(!string.IsNullOrWhiteSpace(to))
                intent.PutExtra(Intent.ExtraEmail, to);

            if(!string.IsNullOrWhiteSpace(subject))
                intent.PutExtra(Intent.ExtraSubject, subject);

            if(!string.IsNullOrWhiteSpace(body))
                intent.PutExtra(Intent.ExtraText, body);

            if (!string.IsNullOrWhiteSpace(cc))
                intent.PutExtra(Intent.ExtraCc, cc);

            //TODO RM: review if this is needed
            intent.SetType("message/rfc822");

            return intent;
        }

        public static string GetStringFromIntent(Intent intent, string extraName)
        {
            if (intent == null || !intent.HasExtra(extraName))
                return string.Empty;

            return intent.GetStringExtra(extraName);
        }

        public static T GetAndDeserializedTypeFromIntent<T>(Intent intent, string extraName) where T : class
        {
            try
            {
                var serializedObject = GetStringFromIntent(intent, extraName);
                
                var result = JsonConvert.DeserializeObject<T>(serializedObject);
    
                return result;

            }
            catch (Exception ex)
            {
                _logger.Error($"Error encountered trying to retrieve and deserialize Type from intent using extraName {extraName}. Returning default for Type {typeof(T).Name}", ex);
                return default(T);
            }
        }

        public static string GetString(int resourceId)
        {
            return BaseApplication.Context.GetString(resourceId);
        }

        public static void SaveTextToClipboard(string label, string textToSave)
        {
            try
            {
                var clipboardManager = Application.Context.GetSystemService(Context.ClipboardService) as ClipboardManager;
                var clip = ClipData.NewPlainText(label, textToSave);
                clipboardManager.PrimaryClip = clip;
            }
            catch (Exception e)
            {
                _logger.Error($"Unable to copy text to clipboard", e);
            }
        }

        #region File Helpers
        public static string GetInternalAppFileContent(string filename)
        {
            var path = Path.Combine(GetInternalAppDirPath(), filename);

            if(!File.Exists(path))
            {
                _logger.Error($"File does not exist, cannot retrieve file contents. Filepath: {filename}");
                return string.Empty;
            }

            return Util.GetFileContents(path);
        }

        public bool SetInternalAppFileContent(string filename, string content)
        {
            var path = Path.Combine(GetInternalAppDirPath(), filename);

            return _util.CreateFileContent(path, content);
        }

        public void CreateInternalDir(string dirName)
        {
            var dirPath = Path.Combine(GetInternalAppDirPath(), dirName);

            Util.CreateDir(dirPath);
        }
        
        public static string GetInternalAppDirPath()
        {
            return Application.Context.FilesDir.Path;
        }

        #endregion


        #region SharedPreferences
        private void SetSharedPreferenceString(string preferenceName, string sharedPreferenceKey, string preferenceValue, FileCreationMode fileCreationMode = FileCreationMode.Private)
        {
            var preferecence = Application.Context.GetSharedPreferences(preferenceName, fileCreationMode);
            var preferenceEdit = preferecence.Edit();
            
            preferenceEdit.PutString(sharedPreferenceKey, preferenceValue);
            preferenceEdit.Commit();
        }

        private string GetSharedPreferenceString(string preferenceName, string sharedPreferenceKey, string preferenceDefaultValue = "", FileCreationMode fileCreationMode = FileCreationMode.Private)
        {
            var preferecence = Application.Context.GetSharedPreferences(preferenceName, fileCreationMode);
            return preferecence.GetString(sharedPreferenceKey, preferenceDefaultValue);
        }
        #endregion

        #endregion
    }
}