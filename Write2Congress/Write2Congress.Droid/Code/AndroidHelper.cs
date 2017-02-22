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
using Android.Graphics;
using Write2Congress.Shared.DomainModel;
using System.Net;
using Write2Congress.Shared.BusinessLayer;

namespace Write2Congress.Droid.Code
{
    public class AndroidHelper
    {
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

        #endregion

        #region AppSpecificHelpers

        public static Intent GetIntentForContactMethod(ContactMethod contactMethod)
        {
            var intent = new Intent(Intent.ActionView);

            switch (contactMethod.Type)
            {
                case Shared.DomainModel.Enum.ContactType.NotSet:
                    break;
                case Shared.DomainModel.Enum.ContactType.Email:
                    return GetIntentForContactEmail(contactMethod);
                case Shared.DomainModel.Enum.ContactType.Phone:
                    return GetIntentForContactPhone(contactMethod);
                case Shared.DomainModel.Enum.ContactType.Mail:
                    return GetIntentForContactAddress(contactMethod);
                case Shared.DomainModel.Enum.ContactType.Facebook:
                case Shared.DomainModel.Enum.ContactType.Twitter:
                case Shared.DomainModel.Enum.ContactType.YouTube:
                case Shared.DomainModel.Enum.ContactType.WebSite:
                case Shared.DomainModel.Enum.ContactType.WebSiteContact:
                    return GetIntentForContactWebsite(contactMethod);
                default:
                    return intent;
            }
            return intent;
        }


        private static Intent GetIntentForContactEmail(ContactMethod contactMethod)
        {
            var intent = new Intent(Intent.ActionSend);
            var to = contactMethod.ContactInfo;
            var subject = "TODO RM";
            var body = "Hello legislator, you suck";

            intent.PutExtra(Intent.ExtraEmail, to);
            intent.PutExtra(Intent.ExtraSubject, subject);
            intent.PutExtra(Intent.ExtraText, body);

            //TODO RM: review if this is needed
            intent.SetType("message/rfc822");

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
            var geoUri = Android.Net.Uri.Parse("geo:0,0?q=" + WebUtility.UrlEncode(contactMethod.ContactInfo));
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
            catch(Exception e)
            {
                imageBitmap = null;
            }

            return imageBitmap;
        }

        #endregion
    }
}