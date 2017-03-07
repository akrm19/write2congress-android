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

namespace Write2Congress.Droid.Activities
{
    [Activity(Label = "WriteLetterActivity")]
    public class WriteLetterActivity : BaseActivity
    {
        private WriteLetterFragment _writeLetterFragment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.actv_WriteLetter);

            _writeLetterFragment = FragmentManager.FindFragmentByTag<WriteLetterFragment>(TagsType.WriteLetterFragment);

            if(_writeLetterFragment == null)
            {
                _writeLetterFragment = new WriteLetterFragment();
                AndroidHelper.AddFragment(FragmentManager, _writeLetterFragment, Resource.Id.writeLetterActv_fragmentContainer, TagsType.WriteLetterFragment);
            }
        }

        private Legislator GetLegislatorFromIntent()
        {
            if (Intent.Extras.ContainsKey(BundleType.Legislator))
            {
                var serializedLegislator = Intent.GetStringExtra(BundleType.Legislator);

                if (string.IsNullOrWhiteSpace(serializedLegislator))
                {
                    try
                    {
                        return new Legislator().DeserializeFromJson(serializedLegislator);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Unable to deserialize legislator from string: {serializedLegislator}");
                        return null;
                    }
                }

                return null;
            }
            return null;
        }
    }
}