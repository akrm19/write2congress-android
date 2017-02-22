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

namespace Write2Congress.Droid.Code
{
    public class AndroidHelper
    {
        public static void AddFragment(FragmentManager fragmentManager, Fragment fragment, int containerId, string tag)
        {
            var transaction = fragmentManager.BeginTransaction();

            transaction.Add(containerId, fragment, tag);
            transaction.Commit();
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

        public static string GetString(int resourceId)
        {
            return BaseApplication.Context.GetString(resourceId);
        }
    }
}