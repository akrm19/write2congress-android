using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write2Congress.Shared.DomainModel
{
    public class Letter
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Signature { get; set; }

        public string RecipientEmail { get; set; }

        public Guid Id { get; set; }
        public bool Sent { get; set; }
        public Legislator Recipient { get; set; }
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
