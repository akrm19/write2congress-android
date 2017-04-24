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
using Android.Support.V7.Preferences;

namespace Write2Congress.Droid.Code
{
    public static class AppHelper
    {
        private static Logger _logger = new Logger("AppHelper");
        private static string _cachedLegislatorsFileName = "Legislators.json";

        //TODO RM: Ensure this works with pre 5.0 like 4.4
        public static Android.Util.TypedValue GetTypedValueFromActv(ContextThemeWrapper activity)
        {
            Android.Util.TypedValue typedValue = new Android.Util.TypedValue();
            try
            {
                return activity.Theme.ResolveAttribute(Android.Resource.Attribute.SelectableItemBackground, typedValue, true)
                    ? typedValue
                    : null;
            }
            catch (Exception e)
            {
                _logger.Error($"An Error occurred while retrieving the SelectableItemBackground used for transparent buttons. {e.Message}");
                return null;
            }
        }


        #region Legislator, Xyz, Populations, 

        public static void SetLegislatorPortrait(Legislator legislator, ImageView imageButton)
        {
            switch (legislator.Party)
            {
                case Party.Democratic:
                    imageButton.SetImageResource(Resource.Drawable.ic_democratic_logo);
                    break;
                case Party.Republican:
                    imageButton.SetImageResource(Resource.Drawable.ic_republican_elephant);
                    break;
                case Party.Independent:
                    imageButton.SetImageResource(Resource.Drawable.ic_person_black_48dp);
                    break;
                case Party.Libertarian:
                    imageButton.SetImageResource(Resource.Drawable.ic_person_black_48dp);
                    break;
                case Party.Green:
                    imageButton.SetImageResource(Resource.Drawable.ic_person_black_48dp);
                    break;
                case Party.Unknown:
                default:
                    imageButton.SetImageResource(Resource.Drawable.ic_person_black_48dp);
                    break;
            }
        }

        public static string GetLegislatorTermStartDate(Legislator legislator, string termStartDateText)
        {
            return legislator.TermStartDate.Equals(DateTime.MinValue)
                    ? $"{termStartDateText}: {AndroidHelper.GetString(Resource.String.unknown)}"
                    : $"{termStartDateText}: {legislator.TermStartDate.ToShortDateString()}";
        }

        public static string GetLegislatorTermEndDate(Legislator legislator, string termEndDateText)
        {
            return legislator.TermEndDate.Equals(DateTime.MinValue)
                ? $"{termEndDateText}: {AndroidHelper.GetString(Resource.String.unknown)}"
                : $"{termEndDateText}: {legislator.TermEndDate.ToShortDateString()}";
        }

        //public static void SetLegislatorContactMthdVisibility(ImageView imageButton, ContactMethod contactMethod, Android.Util.TypedValue selectableItemBackground)
        //{
        //    imageButton.Visibility = contactMethod.IsEmpty
        //        ? ViewStates.Gone
        //        : ViewStates.Visible;
        //
        //    if (selectableItemBackground != null)
        //        imageButton.SetBackgroundResource(selectableItemBackground.ResourceId);
        //}

        public static void SetLegislatorContactMthdVisibility(View imageButton, ContactMethod contactMethod, Android.Util.TypedValue selectableItemBackground)
        {
            imageButton.Visibility = contactMethod.IsEmpty
                ? ViewStates.Gone
                : ViewStates.Visible;

            if (selectableItemBackground != null)
                imageButton.SetBackgroundResource(selectableItemBackground.ResourceId);
        }

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
        #endregion

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
                SetDefaultPreferenceString(SharedPreference.LegislatorsLastUpdate, DateTime.UtcNow.ToString());
            }
            catch (Exception e)
            {
                _logger.Error("An error occurred savind the legislators to cache: Error: {0}", e.Message);
            }
        }
        #endregion


        #region AppPreference Helpers

        public static DateTime GetLastLegislatorUpdate()
        {
            var lastUpdateString = GetDefaultPreferenceString(SharedPreference.LegislatorsLastUpdate);
            DateTime lastUpdate;

            if (string.IsNullOrWhiteSpace(lastUpdateString) || !DateTime.TryParse(lastUpdateString, out lastUpdate))
                return DateTime.MinValue;

            return lastUpdate;
        }

        public static string GetAppPreferenceString(string preferenceKey, string defaultValue = "")
        {
            return GetAppPreferences().GetString(preferenceKey, defaultValue);
        }

        public static bool SetAppPreferenceString(string preferenceName, string value)
        {
            return GetAppPreferences()
                .Edit()
                .PutString(preferenceName, value)
                .Commit();
        }

        public static ISharedPreferences GetAppPreferences()
        {
            var preferenceFileName = AndroidHelper.GetString(Resource.String.app_name) + "Settings";
            var prefereces = Application.Context.GetSharedPreferences(preferenceFileName, FileCreationMode.Private);

            return prefereces;
        }

        public static string GetDefaultPreferenceString(string preferenceKey, string defaultValue = "")
        {
            return GetDefaultSharedPreferences().GetString(preferenceKey, defaultValue);
        }

        public static bool SetDefaultPreferenceString(string preferenceName, string value)
        {
            return GetDefaultSharedPreferences()
                .Edit()
                .PutString(preferenceName, value)
                .Commit();
        }

        public static ISharedPreferences GetDefaultSharedPreferences()
        {
            return PreferenceManager.GetDefaultSharedPreferences(Application.Context);
        }
        #endregion


        #region Intents Methods (ContactMethods & Actions)

        public static void StartViewLegislatorIntent(BaseActivity activity, Legislator legislator)
        {
            using (var intent = new Intent(activity, typeof(ViewLegislatorActivity)))
            {
                if (legislator != null)
                    intent.PutExtra(BundleType.Legislator, legislator.SerializeToJson());

                activity.StartActivity(intent);
            }
        }

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

        public static void PerformContactMethodIntent(Fragments.BaseFragment fragment, ContactMethod contactMethod, bool useChooser)
        {
            var intent = useChooser
                ? Intent.CreateChooser(GetIntentForContactMethod(contactMethod), "Open with")
                : new Intent(GetIntentForContactMethod(contactMethod));

            fragment.StartActivity(intent);
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
            var body = "Hello legislator";

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
        
        public static Legislator GetLegislatorFromIntent(Intent intent, string bundleType = BundleType.Legislator)
        {
            var legislator = AndroidHelper.GetAndDeserializedTypeFromIntent<Legislator>(intent, bundleType);

            if (legislator == null)
                _logger.Error($"Unable to retrieve legislator from intent's {BundleType.Legislator} extra.");

            return legislator;
        }
        #endregion
    }
}