using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.BusinessLayer;
using Write2Congress.Shared.DomainModel.Enum;

namespace Write2Congress.Shared.DomainModel
{
    public class BillStatus
    {
        public BillStatusKind Status;
        public DateTime StatusDate;
        private string _statusText;

        public BillStatus(BillStatusKind status)
            : this(status, DateTime.MinValue)
        {
        }

        public BillStatus(BillStatusKind status, DateTime date)
            : this(status, date, string.Empty)
        {
        }

        public BillStatus(BillStatusKind status, DateTime date, string text)
        {
            Status = status;
            StatusDate = date;
            StatusText = text;
        } 
        
        public string StatusText
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_statusText))
                    return _statusText;

                else if (Status != BillStatusKind.Unknown)
                    return Status.GetDescription(); // .ToString();
                
                return string.Empty;
            }
            set
            {
                _statusText = value;
            }
        }
    }
}
