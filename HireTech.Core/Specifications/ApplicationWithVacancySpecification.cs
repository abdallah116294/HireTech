using HireTech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireTech.Core.Specifications
{
    public class ApplicationWithVacancySpecification:BaseSpecifications<Application>
    {
        public ApplicationWithVacancySpecification(string candidateId):base(p=>p.CandidateId== candidateId)
        {

            Includes.Add(p => p.Candidate);
            Includes.Add(p => p.Vacancy);
            Includes.Add(p => p.Vacancy.Company);
        }
        public ApplicationWithVacancySpecification(int vacancyId) : base(p=>p.VacancyId== vacancyId)
        {
            Includes.Add(p => p.Candidate);
            Includes.Add(p => p.Vacancy);
            Includes.Add(p => p.Vacancy.Company);
        }
        public ApplicationWithVacancySpecification(int candidateProfileId, int vacancyId):
            base(a => a.CandidateProfileId == candidateProfileId && a.VacancyId == vacancyId)
        {
            Includes.Add(p => p.Candidate);
            Includes.Add(p => p.Vacancy);
            Includes.Add(p => p.Vacancy.Company);
        }
    }
}
