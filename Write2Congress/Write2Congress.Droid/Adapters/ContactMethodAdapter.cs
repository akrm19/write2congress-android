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
using Java.Lang;
using Write2Congress.Droid.Fragments;
using Write2Congress.Shared.DomainModel;
using Android.Support.V7.Widget;

namespace Write2Congress.Droid.Adapters
{

    public class ContactMethodAdapter : RecyclerView.Adapter
    {
        private BaseFragment _fragment;
        private List<ContactMethod> _contactMethods = new List<ContactMethod>();

        public ContactMethodAdapter (BaseFragment fragment, List<ContactMethod> contactMethods)
        {
            _fragment = fragment;
            _contactMethods = contactMethods;
        }

        public override int ItemCount
        {
            get
            {
                return _contactMethods.Count;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            throw new NotImplementedException();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            throw new NotImplementedException();
        }

    }
}