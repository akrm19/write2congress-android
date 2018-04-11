using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel.Enum;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.DomainModel
{
    public class Committee : ICommittee
    {
        /// <summary>
        /// Official ID of the committee, as it appears in various official sources (Senate, House, and Library of Congress).
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Official name of the committee. Parent committees tend to have a prefix, e.g. “House Committee on”, 
        /// and subcommittees do not, e.g. “Health”.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The chamber this committee is part of. “house”, “senate”, or “joint”.
        /// </summary>
        public LegislativeBody Chamber { get; set; }

        /// <summary>
        /// The committee’s phone number.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// The committee’s official website.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// If the committee is a subcommittee, the ID of its parent committee.
        /// </summary>
        public string ParentCommitteeId { get; set; }

        /// <summary>
        /// Whether or not the committee is a subcommittee.
        /// </summary>
        public bool IsSubcommittee { get; set; }

        public static Committee FromICommittee(ICommittee committee)
        {
            var newCommittee = new Committee
            {
                Chamber = committee.Chamber,
                Id = committee.Id,
                IsSubcommittee = committee.IsSubcommittee,
                Name = committee.Name,
                ParentCommitteeId = committee.ParentCommitteeId,
                Phone = committee.Phone,
                Url = committee.Url
            };

            return newCommittee;
        }
    }
}
