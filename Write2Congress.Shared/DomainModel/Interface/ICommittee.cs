using System;
using Write2Congress.Shared.DomainModel.Enum;

namespace Write2Congress.Shared.DomainModel.Interface
{
    public interface ICommittee
    {
        /// <summary>
        /// Official ID of the committee, as it appears in various official sources (Senate, House, and Library of Congress).
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Official name of the committee. Parent committees tend to have a prefix, e.g. “House Committee on”, 
        /// and subcommittees do not, e.g. “Health”.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The chamber this committee is part of. “house”, “senate”, or “joint”.
        /// </summary>
        LegislativeBody Chamber { get; }

        /// <summary>
        /// The committee’s phone number.
        /// </summary>
        string Phone { get; set; }

        /// <summary>
        /// The committee’s official website.
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// If the committee is a subcommittee, the ID of its parent committee.
        /// </summary>
        string ParentCommitteeId { get; set; }

        /// <summary>
        /// Whether or not the committee is a subcommittee.
        /// </summary>
        bool IsSubcommittee { get; set; }
    }
}
