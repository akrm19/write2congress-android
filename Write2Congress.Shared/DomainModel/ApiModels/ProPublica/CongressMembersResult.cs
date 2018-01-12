using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write2Congress.Shared.DomainModel.Enum;
using Write2Congress.Shared.DomainModel.Interface;

namespace Write2Congress.Shared.DomainModel.ApiModels.ProPublica
{
    public class CongressMembersResult : BaseLegislatorsResult
    {
        public class Rootobject : BaseRootObject, ILegislatorResult
        {
            public Result[] results { get; set; }

            //TODO RM: look into redoing this, maybe add more inheritance
            List<ILegislator> ILegislatorResult.GetLegislatorsResult()
            {
                var legislators = new List<ILegislator>();

                foreach (var result in results.Where(r => r.members != null && r.members.Count() > 0))
                    legislators.AddRange(result.members);

                return legislators;
            }
        }

        public class Result : BaseResult
        {
            public CongressMember[] members { get; set; }
        }

        public class CongressMember : Member
        {
            public bool at_large { get; set; }
            public string district { get; set; }
            public string geoid { get; set; }

            public CongressMember()
            {
                (this as ILegislator).Chamber = LegislativeBody.House;
            }
        }
    }
}
