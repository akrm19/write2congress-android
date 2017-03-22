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
using Write2Congress.Shared.DomainModel;
using Android.Graphics;
using Write2Congress.Shared.BusinessLayer;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Write2Congress.Shared.DomainModel.Enum;
using Android.Locations;
using Write2Congress.Droid.Activities;
using Write2Congress.Droid.DomainModel.Constants;
using Write2Congress.Droid.DomainModel.Enums;

namespace Write2Congress.Droid.Code
{
    public static class AppHelper
    {
        private static Logger _logger = new Logger("AppHelper");
        private static string _cachedLegislatorsFileName = "Legislators.json";

        public static Bitmap GetPortraitForLegislator(Legislator legislator)
        {
            Bitmap imageBitmap = null;

            if (string.IsNullOrWhiteSpace(legislator.BioguideId))
                return imageBitmap;

            var url = string.Format("https://theunitedstates.io/images/congress/225x275/{0}.jpg", legislator.BioguideId);
            try
            {
                using (var webClient = new WebClient())
                {
                    var imageBytes = webClient.DownloadData(url);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    }
                }
            }
            catch (System.Exception e)
            {
                _logger.Error(string.Format("Unable to retrieve portrait for {0}. Error: {1}", legislator.FullName, e.Message));
                imageBitmap = null;
            }

            return imageBitmap;
        }

        #region StateOrTerritory Helpers

        public static StateOrTerritory GetUsaStateFromAddress(Address address)
        {
            StateOrTerritory defaultStateOrTerritory = StateOrTerritory.ALL;

            if (address == null)
                _logger.Info("Address is null. Cannot retrieve StateOrTerritory. Returning default State: " + defaultStateOrTerritory.ToString());

            else if (!GeoHelper.IsAddressInUs(address))
                _logger.Info($"Address is outside the US ({address.CountryName}). Returning default State: {defaultStateOrTerritory.ToString()}");

            else if (string.IsNullOrWhiteSpace(address.AdminArea))
                _logger.Info($"Address does not contain a State. Returning default State: {defaultStateOrTerritory.ToString()}");

            else
                defaultStateOrTerritory = Util.GetStateOrTerrByDescription(address.AdminArea, defaultStateOrTerritory);

            return defaultStateOrTerritory;
        }

        #endregion

        #region Legislators File Caching

        public static List<Legislator> GetCachedLegislators()
        {
            return ((BaseApplication)Application.Context.ApplicationContext).GetCachedLegislators();
        }

        public static List<Legislator> GetCachedLegislatorsFromFileStorage()
        {
            var cachedLegislators = new List<Legislator>();
            try
            {
                var cachedLegislatorsFileContent = AndroidHelper.GetInternalAppFileContent(_cachedLegislatorsFileName);

                if (string.IsNullOrWhiteSpace(cachedLegislatorsFileContent))
                {
                    _logger.Info("No cached legislators retrieved. Returning empty list.");
                    return cachedLegislators;
                }

                cachedLegislators = JsonConvert.DeserializeObject<List<Legislator>>(cachedLegislatorsFileContent);
            }
            catch (Exception e)
            {
                _logger.Error("Error occurred while retrieving cached legislators. Error: " + e.Message);
            }

            return cachedLegislators;
        }

        public static void SaveLegistorsToFileStorage(List<Legislator> legistlators)
        {
            try
            {
                var serializedLegislators = JsonConvert.SerializeObject(legistlators);
                AndroidHelper.SetInternalAppFileContent(_cachedLegislatorsFileName, serializedLegislators);
            }
            catch (Exception e)
            {
                _logger.Error("An error occurred savind the legislators to cache: Error: {0}", e.Message);
            }
        }
        #endregion

        #region AppPreference Helpers

        public static string GetAppPreferenceString(string preferenceKey, string defaultValue = "")
        {
            return GetAppPreferences().GetString(preferenceKey, defaultValue);
        }

        public static string SetAppPreferenceString(string preferenceName, string defaultValue = "")
        {
            return GetAppPreferences().GetString(preferenceName, defaultValue);
        }

        public static ISharedPreferences GetAppPreferences()
        {
            var preferenceFileName = AndroidHelper.GetString(Resource.String.app_name) + "Settings";
            var prefereces = Application.Context.GetSharedPreferences(preferenceFileName, FileCreationMode.Private);

            return prefereces;
        }

        #endregion

        #region Intents Methods (ContactMethods & Actions)


        public static void StartWriteNewLetterIntent(BaseActivity activity, BundleSenderKind senderKind, Legislator legislator = null, bool finishActivity = false)
        {
            using (var intent = new Intent(activity, typeof(WriteLetterActivity)))
            {
                if (legislator != null)
                    intent.PutExtra(BundleType.Legislator, legislator.SerializeToJson());

                intent.PutExtra(BundleType.Sender, (int)senderKind);

                activity.StartActivity(intent);

                if (finishActivity)
                    activity.Finish();
            }
        }

        public static void StartWriteNewLetterIntent(BaseActivity activity, BundleSenderKind senderKind, Letter letter, bool finishActivity)
        {
            using (var intent = new Intent(activity, typeof(WriteLetterActivity)))
            {
                if (letter != null)
                {
                    var serializedLetter = letter.SerializeToJson();
                    intent.PutExtra(BundleType.Letter, serializedLetter);
                }

                intent.PutExtra(BundleType.Sender, (int)senderKind);

                activity.StartActivity(intent);

                if (finishActivity)
                    activity.Finish();
            }
        }

        public static Intent GetIntentForContactMethod(ContactMethod contactMethod)
        {
            var intent = new Intent(Intent.ActionView);

            switch (contactMethod.Type)
            {
                case ContactType.NotSet:
                    break;
                case ContactType.Email:
                    return GetIntentForContactEmail(contactMethod);
                case ContactType.Phone:
                    return GetIntentForContactPhone(contactMethod);
                case ContactType.Mail:
                    return GetIntentForContactAddress(contactMethod);
                case ContactType.Facebook:
                case ContactType.Twitter:
                case ContactType.YouTube:
                case ContactType.WebSite:
                case ContactType.WebSiteContact:
                    return GetIntentForContactWebsite(contactMethod);
                default:
                    return intent;
            }
            return intent;
        }


        private static Intent GetIntentForContactEmail(ContactMethod contactMethod)
        {
            var to = contactMethod.ContactInfo;
            var subject = "TODO RM";
            var body = "Hello legislator, you suck";

            var intent = AndroidHelper.GetSendEmailIntent(to, subject, body, string.Empty);

            return intent;
        }

        private static Intent GetIntentForContactPhone(ContactMethod contacMethod)
        {
            var uri = Android.Net.Uri.Parse("tel:" + contacMethod.ContactInfo);
            var intent = new Intent(Intent.ActionDial, uri);

            return intent;
        }

        private static Intent GetIntentForContactAddress(ContactMethod contactMethod)
        {
            //var geoUri = "http://maps.google.co.in/maps?q=" + contactMethod.ContactInfo; // WebUtility.UrlEncode(contactMethod.ContactInfo)
            var geoUri = Android.Net.Uri.Parse("geo:0,0?q=" + WebUtility.UrlEncode(contactMethod.ContactInfo + " Washington DC"));
            var intent = new Intent(Intent.ActionView, geoUri);

            return intent;
        }

        private static Intent GetIntentForContactWebsite(ContactMethod contactMethod)
        {
            var intent = new Intent(Intent.ActionView);
            var url = Util.GetUrlFromSocialContactMethod(contactMethod);
            var uri = Android.Net.Uri.Parse(url);

            intent.SetData(uri);
            return intent;
        }
        #endregion
    }
}