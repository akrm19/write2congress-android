
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Fragment = Android.Support.V4.App.Fragment;
using Write2Congress.Droid.CustomControls;
using Write2Congress.Droid.Code;
using Write2Congress.Shared.DomainModel;

namespace Write2Congress.Droid.Fragments
{
    public class FavoriteLegislatorsFragment : BaseFragment
    {
        LegislatorsViewer _legislatorsViewer;


        protected ViewSwitcher viewSwitcher;
        protected TextView emptyText; 
        //protected bool errorOccurred = false;

        public FavoriteLegislatorsFragment() {}

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            HasOptionsMenu = true;

            //Inflate fragment
            var fragment = inflater.Inflate(Resource.Layout.frag_FavoriteLegislators, container, false);

            viewSwitcher = fragment.FindViewById<ViewSwitcher>(Resource.Id.favortieLegislatorsFrag_viewSwitcher);
            emptyText = fragment.FindViewById<TextView>(Resource.Id.favortieLegislatorsFrag_emptyText);
            emptyText.Text = EmptyText();

            //Setup legislatorsViewer
            _legislatorsViewer = fragment.FindViewById<LegislatorsViewer>(Resource.Id.favortieLegislatorsFrag_legislatorsViewer);
            _legislatorsViewer.SetupCtrl(this, new List<Legislator>(), false);

            return fragment;
        }

        public override void OnStart()
        {
            base.OnStart();

            (Activity as Activities.BaseActivity).UpdateTitleBarText(AndroidHelper.GetString(Resource.String.favorites));
        }

        /// <summary>
        /// Fragments in the FragmentPagerAdapter are only detached 
        /// and never removed from the FragmentManager (unless the 
        /// Activity is finished). When using FragmentPagerAdapter 
        /// you must make sure to clear any references to the current 
        /// View or Context in onDestroyView()
        /// </summary>
        public override void OnDestroyView()
        {
            base.OnDestroyView();

            CleanUp();
        }

        protected virtual void CleanUp()
        {
            _legislatorsViewer = null;
            viewSwitcher = null;
            emptyText = null;
        }

        public override void OnResume()
        {
            base.OnResume();

            var favoriteLegislators = new List<Legislator>();

            try
            {
                favoriteLegislators = AppHelper.GetFavoriteLegislators().OrderBy(l => l.LastName).ToList();
            }
            catch(Exception e)
            {
                MyLogger.Error("Error encountered retrieving favorite legislators.", e);
                HandleErrorRetrievingData();
            }

            _legislatorsViewer.UpdateLegislators(favoriteLegislators);
            ShowEmptyviewIfNecessary(favoriteLegislators);
        }

        protected void ShowEmptyviewIfNecessary(List<Legislator> legislators)
        {
            if (legislators.Count > 0)
                _legislatorsViewer.SetupCtrl(this, legislators, false);
            else if (legislators.Count() == 0 && viewSwitcher.NextView.Id == Resource.Id.favortieLegislatorsFrag_emptyText)
                viewSwitcher.ShowNext();

            else if (legislators.Count() > 0 && viewSwitcher.CurrentView.Id != Resource.Id.favortieLegislatorsFrag_legislatorsViewer)
                viewSwitcher.ShowNext();
        }

        protected string EmptyText()
        {
            return AndroidHelper.GetString(Resource.String.noFavoriteLegislators);
        }

        protected virtual void HandleErrorRetrievingData()
        {
            //errorOccurred = true;

            if (emptyText != null)
                emptyText.Text = AndroidHelper.GetString(Resource.String.unableToRetrieveFavLegislators);

            ShowToast(AndroidHelper.GetString(Resource.String.dataRetrievedError), ToastLength.Long);
        }

        protected virtual void HandleSuccessfullDataRetrieval()
        {
            //errorOccurred = false;
        }
    }
}
