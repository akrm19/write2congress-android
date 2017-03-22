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
using Write2Congress.Droid.Code;

namespace Write2Congress.Droid.Fragments
{
    public class WriteLetterFragment : BaseFragment
    {
        private EditText _recipient;
        private EditText _subject;
        private EditText _body;
        private EditText _signature;
        private TextView _lastSaved;
        //private Legislator _selectedLegislator;
        private Letter _currentLetter;
        private System.Timers.Timer _autoSaveTimer;
        private double _autoSaveIntervalInMilliSecs = 60000;

        bool _firstTimeInit = false;

        public WriteLetterFragment()
        {
            if (_currentLetter == null)
                _currentLetter = new Letter();

            _firstTimeInit = true;

            _autoSaveTimer = new System.Timers.Timer(_autoSaveIntervalInMilliSecs);
            _autoSaveTimer.Elapsed += AutoSaveTimer_Elapsed;
            _autoSaveTimer.Enabled = false;
        }

        public WriteLetterFragment(Legislator legislator) : this()
        {
            if(legislator != null)
                _currentLetter = new Letter(legislator);
        }

        public WriteLetterFragment(Letter letter) : this()
        {
            _currentLetter = letter ?? new Letter();
        }

        private void AutoSaveTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SaveCurrentLetter(true);
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

            var toolbar = SetupToolbar(fragment, Resource.Id.writeLetterFrag_toolbar, AndroidHelper.GetString(Resource.String.writeNewLetterTitle));
            toolbar.MenuItemClick += Toolbar_MenuItemClick;

            _recipient = fragment.FindViewById<EditText>(Resource.Id.writeLetterFrag_recipient);
            _subject = fragment.FindViewById<EditText>(Resource.Id.writeLetterFrag_subject);
            _body = fragment.FindViewById<EditText>(Resource.Id.writeLetterFrag_body);
            _signature = fragment.FindViewById<EditText>(Resource.Id.writeLetterFrag_signature);
            _lastSaved = fragment.FindViewById<TextView>(Resource.Id.writeLetterFrag_lastSaved);

            return fragment;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            if (_firstTimeInit)
            {
                PopulateFieldsFromSavedLetter(_currentLetter);
                _firstTimeInit = false;
            }

            _autoSaveTimer.Enabled = true;
        }

        public override void OnResume()
        {
            base.OnResume();

            _autoSaveTimer.Enabled = true;    
        }

        public override void OnPause()
        {
            base.OnPause();

            _autoSaveTimer.Enabled = false;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _autoSaveTimer.Enabled = false;
            _autoSaveTimer.Dispose();
        }

        protected override void Toolbar_MenuItemClick(object sender, Toolbar.MenuItemClickEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.writeLetterMenu_send:
                    SendCurrentLetter();
                    break;
                case Resource.Id.writeLetterMenu_save:
                    SaveCurrentLetter(false);
                    break;
                case Resource.Id.writeLetterMenu_delete:
                    DeleteCurrentLetter();
                    break;
                case Resource.Id.writeLetterMenu_donate:
                    DonatePressed();
                    break;
                case Resource.Id.writeLetterMenu_settings:
                    SettingsPressed();
                    break;
                case Resource.Id.writeLetterMenu_exit:
                    ExitButtonPressed();
                    break;
                default:
                    base.Toolbar_MenuItemClick(sender, e);
                    break;
            }
        }

        private void SendCurrentLetter()
        {
            if(string.IsNullOrWhiteSpace(_recipient.Text))
            {
                ShowToast(GetString(Resource.String.cannotSendLetterWithoutRecipient));
                return;
            }

            var intent = AndroidHelper.GetSendEmailIntent(_recipient.Text, _subject.Text, _body.Text, string.Empty);

            StartActivity(intent);
        }

        private void DeleteCurrentLetter()
        {
            var result = GetLetterManager().DeleteLetterById(_currentLetter.Id.ToString());

            if(result)
                ClearTextFields();

            ShowToast(GetString(result
                ? Resource.String.letterDeleted
                : Resource.String.letterDeleteFailed));
        }

        private void ClearTextFields()
        {
            _recipient.Text = string.Empty;
            _subject.Text = string.Empty;
            _body.Text = string.Empty;
            _signature.Text = string.Empty;
            _lastSaved.Text = string.Empty;
        }

        private void SaveCurrentLetter(bool isAutoSave, bool showToastUpdate = true)
        {
            if (_currentLetter == null)
                return;

            UpdateLetterFromFields(_currentLetter);

            var oldLastSavedTime = _currentLetter.LastSaved;
            _currentLetter.LastSaved = DateTime.Now;

            var letterSaved = GetLetterManager().SaveLetter(_currentLetter);

            if (letterSaved)
            {
                _lastSaved.Text = GetLastSavedText(_currentLetter.LastSaved);

                if(showToastUpdate)
                    ShowToast(GetString(isAutoSave
                        ? Resource.String.letterAutoSaved
                        : Resource.String.letterSaved));
            }
            else
            {
                _currentLetter.LastSaved = oldLastSavedTime;

                if (!isAutoSave && showToastUpdate)
                    ShowToast(GetString(Resource.String.letterSaveFailed));
            }
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.menu_writeLetter, menu);

            base.OnCreateOptionsMenu(menu, inflater);
        }

        private void PopulateFieldsFromSavedLetter(Letter letter)
        {
            if (letter == null)
                return;

            _recipient.Text = string.IsNullOrWhiteSpace(letter.RecipientEmail)
                ? string.Empty
                : letter.RecipientEmail;

            _subject.Text = letter.Subject ?? string.Empty;
            _body.Text = letter.Body ?? string.Empty;
            _signature.Text = letter.Signature ?? string.Empty;

            _lastSaved.Text = (letter.LastSaved == null || letter.LastSaved == DateTime.MinValue)
                ? string.Empty
                : GetLastSavedText(letter.LastSaved);
        }

        private void UpdateLetterFromFields(Letter letter)
        {
            letter.RecipientEmail = _recipient.Text;
            letter.Subject = _subject.Text;
            letter.Body = _body.Text;
            letter.Signature = _signature.Text;
        }

        private string GetLastSavedText(DateTime lastSaved)
        {
            return string.Format("{0}: {1}",
                GetString(Resource.String.lastSaved),
                lastSaved.ToString("G"));
        }
    }
}