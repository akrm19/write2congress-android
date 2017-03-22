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
using Write2Congress.Droid.Fragments;
using Write2Congress.Droid.DomainModel.Constants;
using Write2Congress.Droid.Code;
using Write2Congress.Shared.DomainModel;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Write2Congress.Droid.DomainModel.Enums;

namespace Write2Congress.Droid.Activities
{
    [Activity]
    public class WriteLetterActivity : BaseToolbarActivity
    {
        private WriteLetterFragment _writeLetterFragment;
        private DrawerLayout _drawerLayout;
        //private NavigationView _navigationView; 

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.actv_WriteLetter);
            SetupNavigationMenu(Resource.Id.writeLetterActv_navigationDrawer);

            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.writeLetterActv_parent);
            _writeLetterFragment = SupportFragmentManager.FindFragmentByTag(TagsType.WriteLetterFragment) as WriteLetterFragment;

            if(_writeLetterFragment == null)
            {
                var senderKind = GetSenderKindFromIntent();

                switch (senderKind)
                {
                    case BundleSenderKind.LegislatorViewer:
                        var legislator = GetLegislatorFromIntent();
                        _writeLetterFragment = new WriteLetterFragment(legislator);
                        break;
                    case BundleSenderKind.ViewLettersAdapter:
                        var letter = GetLetterFromIntent();
                        _writeLetterFragment = new WriteLetterFragment(letter);
                        break;
                    default:
                        _writeLetterFragment = new WriteLetterFragment();
                        break;
                }

                AndroidHelper.AddSupportFragment(SupportFragmentManager, _writeLetterFragment, Resource.Id.writeLetterActv_fragmentContainer, TagsType.WriteLetterFragment);
            }
        }

        private BundleSenderKind GetSenderKindFromIntent()
        {
            try
            {
                if (Intent == null || !Intent.HasExtra(BundleType.Sender))
                    return default(BundleSenderKind);

                var senderKindAsInt = Intent.GetIntExtra(BundleType.Sender, 0);
                return (BundleSenderKind)senderKindAsInt;
            }
            catch (Exception ex)
            {
                MyLogger.Error($"Unable to retrieve {BundleType.Sender} from intent Extras. Error {ex.ToString()}");
                return default(BundleSenderKind);
            }
        }

        private Letter GetLetterFromIntent()
        {
            var letter = AndroidHelper.GetSerializedTypeFromIntent<Letter>(Intent, BundleType.Letter);

            if (letter == null)
                MyLogger.Error($"Unable to retrieve letter from intent's {BundleType.Letter} extra.");

            return letter;
            //if (Intent == null || !Intent.HasExtra(BundleType.Letter))
            //    return null;
            //
            //var serialziedLetter = Intent.GetStringExtra(BundleType.Letter);
            //
            //var letter = new Letter().DeserializeFromJson(serialziedLetter);
            //return letter;
        }

        private Legislator GetLegislatorFromIntent()
        {
            var legislator = AndroidHelper.GetSerializedTypeFromIntent<Legislator>(Intent, BundleType.Legislator);

            if (legislator == null)
                MyLogger.Error($"Unable to retrieve legislator from intent's {BundleType.Legislator} extra.");

            return legislator;
            /*
            var serializedLegislator = Intent.Extras.ContainsKey(BundleType.Legislator)
                ? Intent.GetStringExtra(BundleType.Legislator)
                : string.Empty;

            try
            {
                return string.IsNullOrWhiteSpace(serializedLegislator)
                    ? null
                    : new Legislator().DeserializeFromJson(serializedLegislator);
            }
            catch (Exception ex)
            {
                MyLogger.Error($"Unable to deserialize legislator from string: {serializedLegislator}");
                return null;
            }
            */
        }
    }
}