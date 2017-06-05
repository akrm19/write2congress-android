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
using Android.Support.V7.Widget;
using Write2Congress.Shared.DomainModel;
using Write2Congress.Droid.Code;
using Write2Congress.Shared.DomainModel.Enum;
using Write2Congress.Droid.Fragments;

namespace Write2Congress.Droid.Adapters
{
    public class CommitteeAdapter : RecyclerView.Adapter
    {
        private BaseFragment _fragment; 
        private Logger _logger;
        private List<Committee> _committees;
        private string _subcommitte;

        public CommitteeAdapter(List<Committee> committees, BaseFragment fragment) 
        {
            _committees = committees;
        }

        public CommitteeAdapter(BaseFragment fragment)
        {
            _fragment = fragment;
            _logger = new Logger(Class.SimpleName);

            _subcommitte = AndroidHelper.GetString(Resource.String.subcommitte);
        }

        public override int ItemCount
        {
            get
            {
                return _committees.Count; ;
            }
        }

        public void UpdateCommittee(List<Committee> committees)
        {
            _committees = committees;
            NotifyDataSetChanged();

            //_fragment.ShowToast(AndroidHelper.GetString());
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var committeeView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ctrl_Committee, parent, false);
            return new CommitteeAdapterViewHolder(committeeView, OnClick);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var committee = _committees[position] ?? null;
            if(committee == null)
            {
                _logger.Error("Cannot bind Committee. Unable to find committee at position" + position);
                return;
            }

            var viewHolder = holder as CommitteeAdapterViewHolder;
            viewHolder.Name.Text = string.Format("{0}{1}",
                committee.Name,
                committee.IsSubcommittee ? $" ({_subcommitte})" : string.Empty);

            AppHelper.SetButtonTextAndHideifNecessary(viewHolder.Phone, committee.Phone);
            AppHelper.SetButtonTextAndHideifNecessary(viewHolder.Website, committee.Url);
        }

        protected void OnClick(int position, int buttonId)
        {
            var committee = _committees[position] ?? null;

            if(committee == null)
            {
                _logger.Error("Cannot process OnClick. Unable to retreat committee at position " + position);
                return;
            }

            var contactMethod = GetContactMethodFromButtonId(buttonId, committee);
            if(contactMethod.Type == ContactType.NotSet || contactMethod.IsEmpty)
            {
                _logger.Error("Cannot process OnClick. Unable to retrieve a valid ContactMethod for committee");
                return;
            }

            AppHelper.PerformContactMethodIntent(_fragment, contactMethod, false);
        }

        private ContactMethod GetContactMethodFromButtonId(int committeeButtonId, Committee committee)
        {
            switch (committeeButtonId)
            {
                case Resource.Id.committeeCtrl_phone:
                    return new ContactMethod(ContactType.Phone, committee.Phone);
                case Resource.Id.committeeCtrl_webpage:
                    return new ContactMethod(ContactType.WebSite, committee.Url);
                default:
                    return new ContactMethod(ContactType.NotSet, string.Empty);
            }
        }
    }
}