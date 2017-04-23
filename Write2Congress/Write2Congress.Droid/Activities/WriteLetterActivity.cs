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

        protected override int DrawerLayoutId
        {
            get
            {
                return Resource.Id.writeLetterActv_parent;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.actv_WriteLetter);
            SetupToolbar(Resource.Id.writeLetterActv_toolbar);//, AndroidHelper.GetString(Resource.String.writeNewLetterTitle));
            SetupNavigationMenu(Resource.Id.writeLetterActv_navigationDrawer);

            _writeLetterFragment = SupportFragmentManager.FindFragmentByTag(TagsType.WriteLetterFragment) as WriteLetterFragment;

            if(_writeLetterFragment == null)
            {
                var senderKind = GetSenderKindFromIntent();

                _writeLetterFragment = new WriteLetterFragment();
                if (_writeLetterFragment.Arguments == null)
                    _writeLetterFragment.Arguments = new Bundle();

                switch (senderKind)
                {
                    case BundleSenderKind.LegislatorViewer:
                        var legislator = AppHelper.GetLegislatorFromIntent(Intent);
                        _writeLetterFragment.Arguments.PutString(BundleType.Legislator, legislator.SerializeToJson());
                        break;
                    case BundleSenderKind.ViewLettersAdapter:
                        var letter = GetLetterFromIntent();
                        _writeLetterFragment.Arguments.PutString(BundleType.Letter, letter.SerializeToJson());
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
            var letter = AndroidHelper.GetAndDeserializedTypeFromIntent<Letter>(Intent, BundleType.Letter);

            if (letter == null)
                MyLogger.Error($"Unable to retrieve letter from intent's {BundleType.Letter} extra.");

            return letter;
        }

        //private Legislator GetLegislatorFromIntent()
        //{
        //    var legislator = AndroidHelper.GetSerializedTypeFromIntent<Legislator>(Intent, BundleType.Legislator);
        //
        //    if (legislator == null)
        //        MyLogger.Error($"Unable to retrieve legislator from intent's {BundleType.Legislator} extra.");
        //
        //    return legislator;
        //}
    }
}