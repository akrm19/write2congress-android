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
using Android.Graphics;
using Android.Locations;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Shared.BusinessLayer;
using System.IO;

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

        public static string GetString(int resourceId)
        {
            return BaseApplication.Context.GetString(resourceId);
        }

        public static string GetInternalAppFileContent(string filename)
        {
            var path = System.IO.Path.Combine(Application.Context.FilesDir.Path, filename);

            if (File.Exists(path))
                return File.ReadAllText(path);
            else
                return string.Empty;
        }

        public static void SetInternalAppFileContent(string filename, string content)
        {
            var path = System.IO.Path.Combine(Application.Context.FilesDir.Path, filename);
            try
            {
                File.WriteAllText(path, content);
            }
            catch (Exception)
            {
                path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                path = System.IO.Path.Combine(path, filename);

                File.WriteAllText(path, content);
            }
        }

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
    }
}