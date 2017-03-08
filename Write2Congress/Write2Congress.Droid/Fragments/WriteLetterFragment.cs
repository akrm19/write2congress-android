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
using Write2Congress.Droid.DomainModel.Constants;
using Write2Congress.Shared.BusinessLayer;
using Write2Congress.Shared.DomainModel;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Write2Congress.Droid.Fragments
{
    public class WriteLetterFragment : BaseFragment
    {
        private EditText _recipient;
        private EditText _subject;
        private EditText _body;
        private EditText _signature;
        private TextView _lastSaved;
        private Legislator _selectedLegislator;
        private Letter _currentLetter; 

        public WriteLetterFragment(Legislator legislator = null)
        {
            if (legislator != null)
                _selectedLegislator = legislator;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var fragment = inflater.Inflate(Resource.Layout.frag_WriteLetter, container, false);

            var toolbar = SetupToolbar(fragment, Resource.Id.writeLetterFrag_toolbar);
            toolbar.MenuItemClick += Toolbar_MenuItemClick;

            _recipient = fragment.FindViewById<EditText>(Resource.Id.writeLetterFrag_recipient);
            _subject = fragment.FindViewById<EditText>(Resource.Id.writeLetterFrag_subject);
            _body = fragment.FindViewById<EditText>(Resource.Id.writeLetterFrag_body);
            _signature = fragment.FindViewById<EditText>(Resource.Id.writeLetterFrag_signature);
            _lastSaved = fragment.FindViewById<TextView>(Resource.Id.writeLetterFrag_lastSaved);

            if (_selectedLegislator != null)
                PopulateFieldsFromLegislator(_selectedLegislator);

            return fragment;
        }

        private void Toolbar_MenuItemClick(object sender, Toolbar.MenuItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.menu_writeLetter, menu);

            base.OnCreateOptionsMenu(menu, inflater);
        }

        private void PopulateFieldsFromLegislator(Legislator legislator)
        {
            if (!string.IsNullOrWhiteSpace(_recipient.Text))
            {
                _recipient.Text = legislator.Email.IsEmpty
                    ? string.Empty
                    : legislator.Email.ContactInfo;
            }

            if (!string.IsNullOrWhiteSpace(_body.Text))
                return;

            _body.Text = string.Format("{0}: {1}{1}",
                legislator.FormalAddressTitle,
                System.Environment.NewLine);
        }

        private void PopulateFieldsFromSavedLetter(Letter letter)
        {
            if (letter == null)
                return;

            _recipient.Text = letter.Recipient.Email.IsEmpty
                ? string.Empty
                : letter.Recipient.Email.ContactInfo;

            _subject.Text = letter.Body ?? string.Empty;

            _signature.Text = letter.Signature ?? string.Empty;

            _lastSaved.Text = (letter.LastSaved == null || letter.LastSaved == DateTime.MinValue)
                ? string.Empty
                : letter.LastSaved.ToString("G");
        }

        private void UpdateLetterFromFields(Letter letter)
        {
            letter.RecipientEmail = _recipient.Text;
            letter.Subject = _subject.Text;
            letter.Body = _body.Text;
            letter.Signature = _signature.Text;
        }
        
    }
}