using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write2Congress.Shared.DomainModel
{
    public class Letter
    {
        public Letter()
        {
            Id = Guid.NewGuid();
            Sent = false;
            DateCreated = DateTime.Now;
            LastSaved = DateTime.MinValue;
            DateSent = DateTime.MinValue;
        }

        public Letter(Legislator legislator) : this()
        {
            if (legislator == null)
                return;

            Recipient = legislator;

            RecipientEmail = legislator.Email.IsEmpty
                ? string.Empty
                : legislator.Email.ContactInfo;

            Body = string.Format("Dear {0},{1}{1}", 
                legislator.FormalAddressTitle(), 
                Environment.NewLine);
        }

        public string Subject { get; set; }
        public string Body { get; set; }
        public string Signature { get; set; }

        public string RecipientEmail { get; set; }
        public Legislator Recipient { get; set; }

        public Guid Id { get; set; }
        public bool Sent { get; set; }
        public DateTime DateCreated { get; }
        public DateTime LastSaved { get; set; }
        public DateTime DateSent { get; set; }

        public void SetLegislatorsEmailAsRecicpientEmail()
        {
            if(Recipient != null)
            {
                RecipientEmail = Recipient.Email.IsEmpty
                    ? string.Empty
                    : Recipient.Email.ContactInfo;
            }
        }
    }
}
