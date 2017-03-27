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
using Android.Util;
using Write2Congress.Droid.Fragments;
using Write2Congress.Droid.Activities;
using Write2Congress.Shared.BusinessLayer;
using Write2Congress.Droid.DomainModel.Constants;

namespace Write2Congress.Droid.Adapters
{
    public class LetterAdapter : RecyclerView.Adapter
    {
        private Logger _logger;
        private List<Letter> _letters;
        private BaseFragment _fragment;
        private TypedValue _selectableItemBackground = new TypedValue();
        private string _lastSavedDate, _sendDate;

        public event EventHandler<int> LetterClick;
        public event EventHandler<int> CopyLetterClick;
        public event EventHandler<int> CopyLetterSucceeded;
        public event EventHandler<int> DeleteLetterClick;
        public event EventHandler<int> DeleteLetterSucceeded;

        public LetterAdapter(BaseFragment fragment, List<Letter> letters)
        {
            _logger = new Logger(Class.SimpleName);
            _letters = letters;
            _fragment = fragment;
            
            _lastSavedDate = AndroidHelper.GetString(Resource.String.letterSaved);
            _sendDate = AndroidHelper.GetString(Resource.String.send);

            //TODO RM: Ensure this works with pre 5.0 like 4.4
            try
            {
                _fragment.Activity.Theme.ResolveAttribute(Android.Resource.Attribute.SelectableItemBackground, _selectableItemBackground, true);
            }
            catch (Exception e)
            {
                _logger.Error($"An Error occurred while retrieving the SelectableItemBackground used for transparent buttons. {e.Message}");
                _selectableItemBackground = null;
            }
        }

        public override int ItemCount
        {
            get
            {
                return _letters.Count;
            }
        }

        public void UpdateLetters(List<Letter> letters)
        {
            _letters = letters;
            NotifyDataSetChanged();

            _fragment.ShowToast(AndroidHelper.GetString(Resource.String.updatedDraftLetters), ToastLength.Short);
        }

        protected void OnClick(int position)
        {
            LetterClick?.Invoke(this, position);
            //OnClick(position);
        }

        protected void OnCopyLetterClick(int position)
        {
            CopyLetterClick?.Invoke(this, position);
            Copy_Click(position);
        }

        protected void OnCopyLetterSucceeded(int position)
        {
            CopyLetterSucceeded?.Invoke(this, position);
        }

        protected void OnDeleteLetterClick(int position)
        {
            DeleteLetterClick?.Invoke(this, position);
            Delete_Click(position);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var letterView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ctrl_Letter, parent, false);
            return new LetterAdapterViewHolder(letterView, OnClick, OnCopyLetterClick, OnDeleteLetterClick);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var letter = _letters[position];
            var viewHolder = holder as LetterAdapterViewHolder;

            viewHolder.Image.SetBackgroundResource(Resource.Color.accent_purple); 
            viewHolder.Image.SetImageResource(letter.Sent
                ? Resource.Drawable.ic_send_white_48dp
                : Resource.Drawable.ic_drafts_white_48dp);

            viewHolder.Subject.Text = string.IsNullOrWhiteSpace(letter.Subject)
                ? string.Empty
                : letter.Subject;

            //TODO RM: This logic might be wring if they free write the subject
            viewHolder.Recipient.Text = (letter.Recipient == null ||letter.Recipient.Email.IsEmpty)
                ? letter.RecipientEmail
                : letter.Recipient.Email.ContactInfo;

            viewHolder.LastSavedDate.Text = letter.LastSaved == DateTime.MinValue
                ? string.Empty
                : $"{_lastSavedDate}: {letter.LastSaved.ToString("G")}";

            viewHolder.SendDate.Text = letter.Sent
                ? $"{_sendDate}: {letter.DateSent.ToString("G")}"
                : string.Empty;

            if (_selectableItemBackground != null)
            {
                viewHolder.Copy.SetBackgroundResource(_selectableItemBackground.ResourceId);
                viewHolder.Delete.SetBackgroundResource(_selectableItemBackground.ResourceId);
            }
        }

        private void Copy_Click(int position)
        {
            var letter = GetLetterAtPosition(position);

            if(letter == null)
            {
                _logger.Error($"Unable to copy letter. Could not retrive letter at position {position}.");
                _fragment.ShowToast(AndroidHelper.GetString(Resource.String.unableToCopyLetter));
                return;
            }

            var copiedLetter = new Letter()
            {
                Body = letter.Body,
                Id = Guid.NewGuid(),
                DateSent = DateTime.MinValue,
                LastSaved = DateTime.Now,
                Recipient = letter.Recipient,
                RecipientEmail = letter.RecipientEmail,
                Sent = false,
                Signature = letter.Signature,
                Subject = letter.Subject
            };

            if (_fragment.GetBaseApp().LetterManager.SaveLetter(copiedLetter))
            {
                _letters.Insert(0, copiedLetter);
                NotifyItemInserted(0);

                OnCopyLetterSucceeded(position);
                _fragment.ShowToast(AndroidHelper.GetString(Resource.String.letterCopied));
            }
            else
                _fragment.ShowToast(AndroidHelper.GetString(Resource.String.unableToCopyLetter));
        }

        private void Delete_Click(int position)
        {
            var letter = GetLetterAtPosition(position);

            if(letter == null)
            {
                _logger.Error($"Unable to delete letter. Could not retrive letter at position {position}.");
                _fragment.ShowToast(AndroidHelper.GetString(Resource.String.unableToDeleteLetter));
                return;
            }

            if (_fragment.GetBaseApp().LetterManager.DeleteLetterById(letter.Id.ToString()))
            {
                //Position is not zero based
                _letters.RemoveAt(position);
                NotifyItemRemoved(position);
                _fragment.ShowToast(AndroidHelper.GetString(Resource.String.letterDeleted));
                DeleteLetterSucceeded?.Invoke(this, position);
            }
            else
                _fragment.ShowToast(AndroidHelper.GetString(Resource.String.unableToDeleteLetter));            
        }

        public List<Letter> GetLetters()
        {
            return _letters;
        }

        public Letter GetLetterAtPosition(int position)
        {
            return _letters.Count > position
                ? _letters[position]
                : null;
        }
    }
}