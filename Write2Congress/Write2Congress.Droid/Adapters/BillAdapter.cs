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
using Write2Congress.Droid.Fragments;
using Write2Congress.Droid.Code;
using Write2Congress.Shared.DomainModel.Enum;

namespace Write2Congress.Droid.Adapters
{
    public class BillAdapter : RecyclerView.Adapter
    {
        private string dateIntroduced, cosponsors, status, date, summary, lastaction, lastactionDate;

        private BaseFragment _fragment;
        private Logger _logger;
        private List<Bill> _bills = new List<Bill>();

        public BillAdapter(BaseFragment fragment)
        {
            _fragment = fragment;
            _logger = new Logger(Class.SimpleName);

            dateIntroduced = AndroidHelper.GetString(Resource.String.dateIntroduced);
            cosponsors = AndroidHelper.GetString(Resource.String.cosponsorCount);
            status = AndroidHelper.GetString(Resource.String.status);
            date = AndroidHelper.GetString(Resource.String.date);
            summary = AndroidHelper.GetString(Resource.String.summary);
            lastaction = AndroidHelper.GetString(Resource.String.lastAction);
            lastactionDate = AndroidHelper.GetString(Resource.String.lastActionDate);
        }

        public override int ItemCount
        {
            get
            {
                return _bills.Count;
            }
        }

        public void UpdateBill(List<Bill> bills)
        {
            _bills = bills;
            NotifyDataSetChanged();
        }

        private void OnBillClick(int position)
        {
            var bill = _bills[position];

            if(bill == null)
            {
                _logger.Error("Cannot process Bill click event. Unable to fidn bills at position " + position);
                return;
            }

            AppHelper.ShowBillDialog(bill, _fragment);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var billView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ctrl_Bill, parent, false);
            return new BillAdapterViewHolder(billView, OnBillClick);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var bill = _bills[position] ?? null;
            if(bill == null)
            {
                _logger.Error("Cannot bind bill. Unable to find bill at position " + position);
                return;
            }

            var viewHolder = holder as BillAdapterViewHolder;
            viewHolder.Name.Text = bill.GetDisplayTitle;
                
            var billStatus = bill.GetBillStatus;
            switch (billStatus.Status)
            {
                case BillStatusKind.Enacted:
                    viewHolder.Image.SetBackgroundResource(Resource.Color.accent_green);
                    break;
                case BillStatusKind.AwaitingSignature:
                    viewHolder.Image.SetBackgroundResource(Resource.Color.accent_yellow);
                    break;
                case BillStatusKind.Vetoed:
                    viewHolder.Image.SetBackgroundResource(Resource.Color.accent_red);
                    break;
                case BillStatusKind.InCongress:
                case BillStatusKind.Unknown:
                default:
                    viewHolder.Image.SetBackgroundResource(Resource.Color.accent_purple);
                    break;
            }

            AppHelper.SetTextviewTextAndVisibility(viewHolder.DateIntroduced, dateIntroduced, bill.DateIntroduced.ToShortDateString());
            AppHelper.SetTextviewTextAndVisibility(viewHolder.Cosponsors, cosponsors, bill.CosponsorsCount.ToString());
            AppHelper.SetTextviewTextAndVisibility(viewHolder.Summary, summary, bill.Summary);

            if(billStatus.Status == BillStatusKind.Unknown)
            {
                viewHolder.Status.Visibility = ViewStates.Gone;
                viewHolder.StatusDate.Visibility = ViewStates.Gone;

                AppHelper.SetTextviewTextAndVisibility(viewHolder.LastActionDate, lastactionDate, bill.LastAction.Date == DateTime.MinValue
                    ? string.Empty
                    : bill.LastAction.Date.ToShortDateString());
                AppHelper.SetTextviewTextAndVisibility(viewHolder.LastActionText, lastaction, bill.LastAction.Text);
            }
            else
            {
                viewHolder.LastActionDate.Visibility = ViewStates.Gone;
                viewHolder.LastActionText.Visibility = ViewStates.Gone;

                AppHelper.SetTextviewTextAndVisibility(viewHolder.StatusDate, date, billStatus.StatusDate == DateTime.MinValue
                    ? string.Empty
                    : billStatus.StatusDate.ToShortDateString());
                AppHelper.SetTextviewTextAndVisibility(viewHolder.Status, status, billStatus.StatusText);
            }
        }
    }
}