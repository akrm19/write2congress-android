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

        #region General Android Helpers

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

        public static T GetSerializedTypeFromIntent<T>(Intent intent, string extraName) where T : class
        {
            try
            {
                if (intent == null || !intent.HasExtra(extraName))
                    return default(T);

                var serializedObject = intent.GetStringExtra(extraName);
                
                var result = JsonConvert.DeserializeObject<T>(serializedObject);
    
                return result;

            }
            catch (Exception ex)
            {
                //TODO RM: adding logging
                return default(T);
            }
        }

        public static string GetString(int resourceId)
        {
            return BaseApplication.Context.GetString(resourceId);
        }

        #region File Helpers
        public static string GetInternalAppFileContent(string filename)
        {
            var path = Path.Combine(GetInternalAppDirPath(), filename);

            return Util.GetFileContents(path);
        }

        public static bool SetInternalAppFileContent(string filename, string content)
        {
            var path = Path.Combine(GetInternalAppDirPath(), filename);

            return Util.CreateFileContent(path, content);
        }

        public static void CreateInternalDir(string dirName)
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