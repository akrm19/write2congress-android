using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel.Enum;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.DomainModel.ApiModels.ProPublica
{
    public class SenateMembersResult : BaseLegislatorsResult
    {
        public class Rootobject : BaseRootObject, ILegislatorResult
        {
            public Result[] results { get; set; }

            List<ILegislator> ILegislatorResult.GetLegislatorsResult()
            {
                var legislators = new List<ILegislator>();

                foreach(var result in results.Where(r => r.members != null && r.members.Count() > 0))
                    legislators.AddRange(result.members);

                return legislators;
            }
        }

        public class Result : BaseResult
        {
            public SenateMember[] members { get; set; }
        }

        public class SenateMember : Member
        {
            public string lis_id { get; set; }
            public string senate_class { get; set; }
            public string state_rank { get; set; }

            public SenateMember()
            {
                (this as ILegislator).Chamber = LegislativeBody.Senate;
            }
        }
    }
}
