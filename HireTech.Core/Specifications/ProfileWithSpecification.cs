using HireTech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.Specifications
{
    public class ProfileWithSpecification:BaseSpecifications<CandidateProfile>
    {
        public ProfileWithSpecification(string userId):base(p=>p.UserId==userId)
        {
            Includes.Add(p => p.Skills);
            Includes.Add(p => p.Applications);
            Includes.Add(p => p.User);
        }
    }
}
