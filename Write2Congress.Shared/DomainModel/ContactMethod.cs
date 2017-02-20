using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel.Enum;

namespace Write2Congress.Shared.DomainModel
{
    public class ContactMethod
    {
        public string ContactInfo { get; set; }
        public ContactType Type { get; set; }
        
        public ContactMethod(ContactType type, string contactInfo)
        {
            Type = type;
            ContactInfo = contactInfo;
        }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrWhiteSpace(ContactInfo);
            }
        }
    }
}
